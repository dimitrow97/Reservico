using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Reservico.Identity.UserPasswordManager.Models;
using Reservico.Api.Controllers.Identity.Models;
using Reservico.Common.Models;
using Reservico.Identity.UserManager.Models;
using Reservico.Services.Reservations;
using Reservico.Services.Reservations.Models;

namespace Reservico.Api.Controllers.Common
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "Common Endpoints")]
    public class ReservationController : BaseCommonController
    {
        private readonly IReservationService reservationService;
        private readonly ILogger<ReservationController> logger;

        public ReservationController(
            IReservationService reservationService,
            ILogger<ReservationController> logger)
        {
            this.reservationService = reservationService;
            this.logger = logger;
        }
        
        [HttpGet("GetAll")]
        [Authorize]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<ReservationViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(Guid? clientId)
        {
            try
            {
                var response = await this.reservationService.GetAll(clientId);

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

        [HttpGet("{reservationId}")]
        [Authorize]
        [ProducesResponseType(typeof(ServiceResponse<ReservationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid reservationId)
        {
            try
            {
                var response = await this.reservationService.Get(reservationId);

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