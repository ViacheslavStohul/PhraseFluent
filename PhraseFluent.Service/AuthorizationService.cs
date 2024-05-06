using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Repositories.Interfaces;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Exceptions;
using PhraseFluent.Service.Extensions;
using PhraseFluent.Service.Options;

namespace PhraseFluent.Service;

public partial class AuthorizationService(
    IOptions<TokenConfiguration> tokenConfiguration,
    IUserRepository userRepository,
    SymmetricSecurityKey signingKey,
    IMapper mapper) : IAuthorizationService
{
    private readonly TokenConfiguration _tokenConfiguration = tokenConfiguration.Value;
    
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="userToCreate">The user to create.</param>
    /// <returns>The token response for the registered user.</returns>
    /// <exception cref="ValidationException">Thrown if the passwords don't match.</exception>
    public async Task<TokenResponse> RegisterUser(UserCreationRequest userToCreate)
    {
        if (userToCreate.Password != userToCreate.RepeatedPassword)
        {
            throw new ForbiddenException("Passwords dont match");
        }

        if (userToCreate.Username.Length < 6 || userToCreate.Username.Contains(' '))
        {
            throw new ForbiddenException("Username cannot contain spaces and has to be at least 8 digits");
        }
        
        IsValidPassword(userToCreate.Password);

        if (userRepository.IsUserNameOccupied(userToCreate.Username))
        {
            throw new ForbiddenException($"Username {userToCreate.Username} is occupied");
        }

        var userEntity = new User
        {
            Uuid = Guid.NewGuid(),
            Username = userToCreate.Username,
            ClientSecret = userToCreate.Password.Hash(),
            ImageUrl = userToCreate.ImageUrl,
            RegistrationDate = DateTime.Now
        };
        
        userRepository.Add(userEntity);

        await userRepository.SaveChangesAsync();

        var tokenId = Guid.NewGuid().ToString();

        var token = GetToken(userEntity.Uuid, tokenId);

        var userSession = new UserSession
        {
            Uuid = Guid.NewGuid(),
            UserId = userEntity.Id,
            RefreshToken = token.RefreshToken,
            JwtId = tokenId,
            RefreshTokenExpiration = DateTime.Now.AddDays(_tokenConfiguration.RefreshTokenExpirationDays),
            Redeemed = false
        };
        
        userRepository.Add(userSession);

        await userRepository.SaveChangesAsync();

        return token;
    }

    /// <summary>
    /// Authorizes a client user based on the provided client data.
    /// </summary>
    /// <param name="clientData">The client data used for authorization.</param>
    /// <returns>The token response if successful.</returns>
    /// <exception cref="ValidationException">Thrown if the client data is invalid.</exception>
    public async Task<TokenResponse> Authorize(UserAuthorizationRequest clientData)
    {
        var user = await userRepository.GetUserByUsername(clientData.Username);

        if (user == null)
        {
            throw new ForbiddenException($"Unknown username {clientData.Username}");
        }

        if (!clientData.Password.IsHashStringsEqual(user.ClientSecret))
        {
            throw new ForbiddenException($"Passwords don't match for {clientData.Username}");
        }

        var tokenId = Guid.NewGuid().ToString();

        var token = GetToken(user.Uuid, tokenId);

        var userSession = new UserSession
        {
            Uuid = Guid.NewGuid(),
            UserId = user.Id,
            RefreshToken = token.RefreshToken,
            JwtId = tokenId,
            RefreshTokenExpiration = DateTime.Now.AddDays(_tokenConfiguration.RefreshTokenExpirationDays),
        };
        
        userRepository.Add(userSession);

        await userRepository.SaveChangesAsync();

        return token;
    }

    /// <summary>
    /// Refreshes the access token by validating the provided access token and refresh token
    /// </summary>
    /// <param name="accessToken">The access token to be refreshed</param>
    /// <param name="refreshToken">The refresh token associated with the access token</param>
    /// <returns>The refreshed access token</returns>
    public async Task<TokenResponse> RefreshTokens(string accessToken, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ForbiddenException("Invalid token");
        }

        var storedToken = await userRepository.GetSessionByRefreshToken(refreshToken);

        if (storedToken == null)
            throw new ForbiddenException("Invalid refresh token");

        if (storedToken.Redeemed)
            throw new ForbiddenException("Refresh token already used");

        
        var jwtId = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.ToString();

        var userUuid = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (jwtId == null || userUuid == null || !string.Equals(storedToken.User.Uuid.ToString(), userUuid, StringComparison.CurrentCultureIgnoreCase) ||
            !string.Equals("jti: " + storedToken.JwtId, jwtId, StringComparison.CurrentCultureIgnoreCase) || DateTime.Now > storedToken.RefreshTokenExpiration)
        {
            throw new ForbiddenException("Invalid access token");
        }

        var newJwtId = Guid.NewGuid().ToString();
        
        var token = GetToken(storedToken.User.Uuid, newJwtId);

        var userSession = new UserSession
        {
            Uuid = Guid.NewGuid(),
            UserId = storedToken.User.Id,
            RefreshToken = token.RefreshToken,
            JwtId = newJwtId,
            RefreshTokenExpiration = DateTime.Now.AddDays(_tokenConfiguration.RefreshTokenExpirationDays),
        };
        
        userRepository.Add(userSession);

        storedToken.Redeemed = true;
        
        userRepository.Update(storedToken);

        await userRepository.SaveChangesAsync();

        return token;
    }
    
    /// <summary>
    /// Generates a new token for the specified user.
    /// </summary>
    /// <param name="userUuid">The unique identifier of the user.</param>
    /// <param name="jwtId">The unique identifier of JWT token that will be created.</param>
    /// <returns>A TokenResponse object containing the access token, refresh token, and expiration time.</returns>
    private TokenResponse GetToken(Guid userUuid, string jwtId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userUuid.ToString()),
            new(JwtRegisteredClaimNames.Jti, jwtId),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };
        
        var now = DateTime.UtcNow;
        var expiry = now.AddMinutes(_tokenConfiguration.ExpiryMinutes);
        var expiresIn = (int)(expiry - now).TotalSeconds;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiry,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _tokenConfiguration.Issuer,
            Audience = _tokenConfiguration.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        var tokenResponse = new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(token),
            RefreshToken = GenerateRefreshToken(),
            ExpiresIn = expiresIn
        };

        return tokenResponse;
    }

    /// <summary>
    /// Get the validation parameters for token validation.
    /// </summary>
    /// <returns>A new instance of <see cref="TokenValidationParameters"/> with the required validation settings.</returns>
    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = _tokenConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = _tokenConfiguration.Audience,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };
    }

    /// <summary>
    /// Generates a refresh token using a cryptographic random number generator
    /// </summary>
    /// <returns>
    /// A string representing the generated refresh token
    /// </returns>
    /// <remarks>
    /// This method generates a refresh token by creating a byte array of length 32 using a cryptographic random number generator.
    /// The byte array is then converted to a base64 string representation and returned.
    /// </remarks>
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Validates whether the provided password is valid based on the specified constraints.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <exception cref="ValidationException">
    /// Thrown when the password is empty, less than 8 characters long, contains spaces, does not contain at least 1 capital letter, or does not contain at least 1 digit.
    /// </exception>
    private static void IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ForbiddenException("Password cannot be empty");

        if (password.Length < 8)
            throw new ForbiddenException("Password must be at least 8 digits");
        
        if (password.Contains(' '))
            throw new ForbiddenException("Password must not contain space");

        if (!LettersConstraint().IsMatch(password))
            throw new ForbiddenException("Password must contain at least 1 capital letter");

        if (!DigitsConstraint().IsMatch(password))
            throw new ForbiddenException("Password must contain at least 1 digit");
    }

    [GeneratedRegex("[A-Z]")]
    private static partial Regex LettersConstraint();
    
    [GeneratedRegex("[0-9]")]
    private static partial Regex DigitsConstraint();
}