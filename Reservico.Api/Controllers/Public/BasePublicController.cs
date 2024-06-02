using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reservico.Api.Controllers.Public
{
    [ApiController]
    [Route("Public/[controller]")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Public Endpoints")]
    public abstract class BasePublicController : ControllerBase
    {
    }
}