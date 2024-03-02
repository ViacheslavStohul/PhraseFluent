using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace PhraseFluent.API.Controllers;

public class BaseController : Controller
{
    public Guid? UserId
    {
        get
        {
            var idString = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (idString == null) return null;

            return Guid.Parse(idString);
        }
    }
}