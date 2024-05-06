using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;

namespace PhraseFluent.API.Controllers;

[Authorize]
[Microsoft.AspNetCore.Components.Route("api/user")]
public class UserController (IUserService userService) : BaseController
{
    [HttpPut]
    [Route("img")]
    public async Task<IActionResult> UpdateUserImage(string imageUrl)
    {
        await userService.UpdateUserImage(imageUrl, UserId.Value);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetUserByUuid([FromQuery] Guid? uuid)
    {
        var user = await userService.GetUserByUuid(uuid, UserId);
        return Ok(user);
    }
}