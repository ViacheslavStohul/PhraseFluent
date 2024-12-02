using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DistLearning.Service;
using DistLearning.Service.DTO.Requests;
using DistLearning.Service.DTO.Responses;
using DistLearning.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace DistLearning.API.Controllers;

using DistLearning.API.ExceptionHandling;
using Microsoft.AspNetCore.RateLimiting;

[Route("api/auth")]
public class AuthorizationController(IAuthorizationService authorizationService)
    : BaseController
{
    [Route("token")]
    [HttpPost]
    [EnableRateLimiting("TokenPolicy")]
    [SwaggerResponse(200, "Returns access and refresh token", typeof(TokenResponse))]
    [SwaggerResponse(400, "Error getting response", typeof(ErrorDetails))]
    [Produces<TokenResponse>]
    public async Task<IActionResult> Authorize([FromBody] UserAuthorizationRequest userData)
    {
        var token = await authorizationService.Authorize(userData);
        return Ok(token);
    }

    [Route("register")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token", typeof(TokenResponse))]
    [SwaggerResponse(400, "Error registing user", typeof(ErrorDetails))]
    [Produces<TokenResponse>]
    public async Task<IActionResult> Register([FromBody] UserCreationRequest userData)
    {
        //var token = await authorizationService.RegisterUser(userData);
        //return Ok(token);
        
        var error = new ErrorDetails
        {
            StatusCode = 403,
            Message = "Функція тимчасово заблокована з міркувань безпеки"
        };
        return new ContentResult
        {
            Content = error.ToString(),
            ContentType = "application/json",
            StatusCode = 403
        };
    }

    [Route("token/refresh")]
    [HttpPost]
    [EnableRateLimiting("TokenPolicy")]
    [SwaggerResponse(200, "Returns access and refresh token", typeof(TokenResponse))]
    [SwaggerResponse(400, "Error refreshing token", typeof(ErrorDetails))]
    [Produces<TokenResponse>]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest userData)
    {
        var token = await authorizationService.RefreshTokens(userData.AccessToken, userData.RefreshToken);
        return Ok(token);
    }
}