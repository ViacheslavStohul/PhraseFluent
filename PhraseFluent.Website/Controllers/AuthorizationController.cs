using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Route("api/auth")]
public class AuthorizationController(IAuthorizationService authorizationService)
    : BaseController
{
    [Route("token")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token")]
    [SwaggerResponse(400, "Error getting response")]
    [Produces<TokenResponse>]
    public async Task<IActionResult> Authorize([FromBody] UserAuthorizationRequest userData)
    {
        var token = await authorizationService.Authorize(userData);
        return Ok(token);
    }

    [Route("register")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token")]
    [SwaggerResponse(400, "Error registing user")]
    [Produces<TokenResponse>]
    public async Task<IActionResult> Register([FromBody] UserCreationRequest userData)
    {
        var token = await authorizationService.RegisterUser(userData);
        return Ok(token);
    }

    [Route("token/refresh")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token")]
    [SwaggerResponse(400, "Error refreshing token")]
    [Produces<TokenResponse>]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest userData)
    {
        var token = await authorizationService.RefreshTokens(userData.AccessToken, userData.RefreshToken);
        return Ok(token);
    }
}