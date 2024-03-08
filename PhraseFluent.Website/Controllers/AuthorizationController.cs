using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Route("api/auth")]
public class AuthorizationController(ILogger<WordController> logger, IAuthorizationService authorizationService) : BaseController
{
    [Route("token")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token")]
    [SwaggerResponse(400, "Error getting response")]
    [Produces<TokenResponse>]
    public async Task<IActionResult> Authorize([FromBody] UserAuthorizationRequest userData)
    {
        try
        {
            var token = await authorizationService.Authorize(userData);
            return Ok(token);
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating token {Exception}", ex.Message);
            return BadRequest();
        }
    }
    
    [Route("register")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token")]
    [SwaggerResponse(400, "Error registing user")]
    [Produces<TokenResponse>]
    public async Task<IActionResult> Register([FromBody] UserCreationRequest userData)
    {
        try
        {
            var token = await authorizationService.RegisterUser(userData);
            return Ok(token);
        }
        catch (ValidationException ex)
        {
            logger.LogError("Error creating token {Exception}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating token {Exception}", ex.Message);
            return BadRequest();
        }
    }
    
    [Route("token/refresh")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token")]
    [SwaggerResponse(400, "Error refreshing token")]
    [Produces<TokenResponse>]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest userData)
    {
        try
        {
            var token = await authorizationService.RefreshTokens(userData.AccessToken, userData.RefreshToken);
            return Ok(token);
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating token {Exception}", ex.Message);
            return BadRequest();
        }
    }
}