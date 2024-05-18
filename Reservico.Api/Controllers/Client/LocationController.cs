using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserManager.Models;
using Reservico.Services.Clients;
using Reservico.Services.Locations;
using Reservico.Services.Locations.Models;

namespace Reservico.Api.Controllers.Client
{
    public class LocationController : BaseClientController
    {
        private readonly IUserManager userManager;
        private readonly ILocationService locationService;
        private readonly ILogger<LocationController> logger;

        public LocationController(
            IUserManager userManager,
            ILocationService locationService,
            ILogger<LocationController> logger)
        {
            this.userManager = userManager;
            this.locationService = locationService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new Location to the Current User`s Client.
        /// </summary>
        /// <param name="model">Location Model to Create. ClientId property should be null.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLocation(CreateLocationRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var userId = this.GetUserId();

                var userClient = await this.userManager.GetUserSelectedClient(new Guid(userId));

                if (!userClient.IsSuccess)
                {
                    return BadRequest(userClient.ErrorMessage);
                }

                model.ClientId = userClient.Data.Id;

                var response = await this.locationService.Create(model);

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
        /// Updates an already existing Location to the Current User`s Client.
        /// </summary>
        /// <param name="model">Location Model to Update.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocation(UpdateLocationRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var response = await this.locationService.Update(model);

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
        /// Gets an existing Location to the Current User`s Client.
        /// </summary>
        /// <param name="model">Location Id to Get.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpGet("{locationId}")]
        [ProducesResponseType(typeof(ServiceResponse<LocationDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<LocationDetailsViewModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid locationId)
        {       
            try
            {
                var response = await this.locationService.Get(locationId);

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
        /// Gets all existing Locations to the Current User`s Client.
        /// </summary>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<LocationViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<LocationViewModel>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = this.GetUserId();

                var userClient = await this.userManager.GetUserSelectedClient(new Guid(userId));

                if (!userClient.IsSuccess)
                {
                    return BadRequest(userClient.ErrorMessage);
                }

                var response = await this.locationService.GetLocations(userClient.Data.Id);

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
        /// Deletes existing Location.
        /// </summary>
        /// <param name="locationId">Id of the location to delete.</param>
        /// <returns>Status code indicating Success or Failure.</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLocation(Guid locationId)
        {
            try
            {
                var response = await this.locationService.Delete(locationId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response.ErrorMessage);
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
