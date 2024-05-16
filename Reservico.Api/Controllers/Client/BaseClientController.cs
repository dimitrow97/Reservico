using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Reservico.Identity.UserManager;

namespace Reservico.Api.Controllers.Client
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = $"{IdentityRoles.ReadOnlyUserRole}, {IdentityRoles.ReadWriteUserRole}")]
    [ApiExplorerSettings(GroupName = "Client Endpoints")]
    public abstract class BaseClientController : ControllerBase
    {
        protected string GetUserId()
            => HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

        //TODO Investigate
        protected string GetClientId()
            => HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ??
               HttpContext.User.Claims
                   .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
    }
}