using System.ComponentModel.DataAnnotations;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service.Interfaces;

public interface IAuthorizationService
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="userToCreate">The user to create.</param>
    /// <returns>The token response for the registered user.</returns>
    /// <exception cref="ValidationException">Thrown if the passwords don't match.</exception>
    /// <exception cref="InvalidDataException">Thrown if username is occupied.</exception>
    Task<TokenResponse> RegisterUser(UserCreationRequest userToCreate);

    /// <summary>
    /// Authorizes a client user based on the provided client data.
    /// </summary>
    /// <param name="clientData">The client data used for authorization.</param>
    /// <returns>The token response if successful.</returns>
    /// <exception cref="ValidationException">Thrown if the client data is invalid.</exception>
    Task<TokenResponse> Authorize(UserAuthorizationRequest clientData);

    /// <summary>
    /// Refreshes the access token by validating the provided access token and refresh token
    /// </summary>
    /// <param name="accessToken">The access token to be refreshed</param>
    /// <param name="refreshToken">The refresh token associated with the access token</param>
    /// <returns>The refreshed access token</returns>
    Task<TokenResponse> RefreshTokens(string accessToken, string refreshToken);
}