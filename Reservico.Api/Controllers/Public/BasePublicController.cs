using Microsoft.AspNetCore.Mvc;

namespace Reservico.Api.Controllers.Public
{
    [ApiController]
    [Route("Public/[controller]")]
    [ApiExplorerSettings(GroupName = "Public Endpoints")]
    public abstract class BasePublicController : ControllerBase
    {
    }
}