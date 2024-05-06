using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using PhraseFluent.Service.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Authorize]
[Microsoft.AspNetCore.Components.Route("api/user")]
public class UserController (IUserService userService) : BaseController
{
    [HttpPut]
    [Route("img")]
    [SwaggerResponse(200, Description = "Changes user avatar")]
    public async Task<IActionResult> UpdateUserImage(string imageUrl)
    {
        await userService.UpdateUserImage(imageUrl, UserId.Value);
        return Ok();
    }

    [HttpGet]
    [SwaggerResponse(200, Description = "Gets user by id")]
    [Produces<UserResponse>]
    public async Task<IActionResult> GetUserByUuid([FromQuery] Guid? uuid)
    {
        var user = await userService.GetUserByUuid(uuid, UserId);
        return Ok(user);
    }
}