using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Services.Dashboard;
using Reservico.Services.Dashboard.Models;

namespace Reservico.Api.Controllers.Common
{
    public class DashboardController : BaseCommonController
    {
        private readonly IDashboardService dashboardService;
        private readonly ILogger<DashboardController> logger;

        public DashboardController(
            IDashboardService dashboardService,
            ILogger<DashboardController> logger)
        {
            this.dashboardService = dashboardService;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ServiceResponse<DashboardViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid? clientId)
        {
            try
            {
                var response = await this.dashboardService.Get(clientId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
