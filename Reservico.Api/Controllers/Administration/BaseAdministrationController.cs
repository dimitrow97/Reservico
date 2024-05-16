using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Reservico.Identity.UserManager;

namespace Reservico.Api.Controllers.Administration
{
    [ApiController]
    [Route("Admin/[controller]")]
    [Authorize(IdentityRoles.AdminRole)]
    [ApiExplorerSettings(GroupName = "Administration Endpoints")]
    public abstract class BaseAdministrationController : ControllerBase
    {
        protected string GetUserId()
            => HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

        protected string GetClientId()
            => HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
    }
}
