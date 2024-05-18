using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Services.Reservations;

namespace Reservico.Api.Controllers.Client
{
    public class ReservationController : BaseClientController
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

        /// <summary>
        /// Confirms a Reservation.
        /// </summary>
        /// <param name="reservationId"> Id of the Reservation to Confirm.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPost("Confirm")]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Confirm(Guid reservationId)
        {         
            try
            {               
                var response = await this.reservationService.Confirm(reservationId);

                if (!response.IsSuccess)
                {
                    return this.BadRequest(response);
                }

                return this.Ok(response);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }        
    }
}