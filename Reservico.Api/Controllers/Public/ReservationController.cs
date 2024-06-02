using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Services.Reservations;
using Reservico.Services.Reservations.Models;

namespace Reservico.Api.Controllers.Public
{
    public class ReservationController : BasePublicController
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
        /// Make a Reservation for the desired Location.
        /// </summary>
        /// <param name="model">Reservation Model to Create.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MakeReservation(MakeReservationRequestModel model)
        {         
            try
            {               
                var response = await this.reservationService.MakeReservation(model);

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

        [HttpGet("{reservationId}")]
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
        
        [HttpPost("Cancellation")]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendCancellation(Guid reservationId)
        {
            try
            {
                var response = await this.reservationService
                    .SendCancellationEmail(reservationId);

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

        /// <summary>
        /// Cancel a Reservation for the desired Location.
        /// </summary>
        /// <param name="reservationId">Id of the Reservation Cancel.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cancel(Guid reservationId)
        {
            try
            {
                var response = await this.reservationService.Cancel(reservationId);

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