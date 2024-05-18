using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserManager.Models;
using Reservico.Services.Clients;
using Reservico.Services.Locations;
using Reservico.Services.Locations.Models;

namespace Reservico.Api.Controllers.Client
{
    public class TableController : BaseClientController
    {
        private readonly IUserManager userManager;
        private readonly ILocationService locationService;
        private readonly ILogger<TableController> logger;

        public TableController(
            IUserManager userManager,
            ILocationService locationService,
            ILogger<TableController> logger)
        {
            this.userManager = userManager;
            this.locationService = locationService;
            this.logger = logger;
        }

        /// <summary>
        /// Adds a new Table to a Location.
        /// </summary>
        /// <param name="model">Table Model to Create.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTable(AddTableRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {               
                var response = await this.locationService.AddTable(model);

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
        /// Updates an already existing Table for a Location.
        /// </summary>
        /// <param name="model">Table Model to Update.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTable(UpdateTableRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var response = await this.locationService.UpdateTable(model);

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
        /// Deletes existing Table.
        /// </summary>
        /// <param name="tableId">Id of the table to delete.</param>
        /// <returns>Status code indicating Success or Failure.</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTable(Guid tableId)
        {
            try
            {
                var response = await this.locationService.DeleteTable(tableId);

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
