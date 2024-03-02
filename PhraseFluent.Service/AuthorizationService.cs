using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Options;

namespace PhraseFluent.Service;

public class AuthorizationService(IOptions<TokenConfiguration> tokenConfiguration, SymmetricSecurityKey signingKey) : IAuthorizationService
{
    private readonly TokenConfiguration _tokenConfiguration = tokenConfiguration.Value;

    public TokenResponse GetToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            //new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };
        
        var now = DateTime.UtcNow;
        var expiry = now.AddMinutes(_tokenConfiguration.ExpiryMinutes);
        var expiresIn = new DateTimeOffset(expiry).ToUnixTimeSeconds();

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
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}