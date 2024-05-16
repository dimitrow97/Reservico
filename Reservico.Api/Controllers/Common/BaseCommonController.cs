using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Reservico.Api.Controllers.Common
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Common Endpoints")]
    public class BaseCommonController : ControllerBase
    {
        protected string GetUserId()
            => HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

        protected string GetClientId()
            => HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
    }
}