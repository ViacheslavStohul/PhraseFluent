using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PhraseFluent.API.Controllers;

public class BaseController : Controller
{
    public Guid? UserId
    {
        get
        {
            var idString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (idString == null) return null;

            return Guid.Parse(idString);
        }
    }
}