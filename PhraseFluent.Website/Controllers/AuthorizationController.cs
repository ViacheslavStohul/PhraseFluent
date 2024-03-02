using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using PhraseFluent.Service.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Route("/auth")]
public class AuthorizationController(ILogger<WordController> logger, IAuthorizationService authorizationService) : BaseController
{
    [Route("/token")]
    [HttpPost]
    [SwaggerResponse(200, "Returns access and refresh token")]
    [SwaggerResponse(400, "Error getting response")]
    [Produces<TokenResponse>]
    public IActionResult Authorize()
    {
        try
        {
            var token = authorizationService.GetToken();
            return Ok(token);
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating token {Exception}", ex.Message);
            return BadRequest();
        }
    }
}