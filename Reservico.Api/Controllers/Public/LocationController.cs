using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Services.LocationImages;
using Reservico.Services.Locations;
using Reservico.Services.Locations.Models;

namespace Reservico.Api.Controllers.Public
{
    public class LocationController : BasePublicController
    {
        private readonly ILocationService locationService;
        private readonly ILocationImagesService locationImagesService;
        private readonly ILogger<LocationController> logger;

        public LocationController(
            ILocationService locationService,
            ILocationImagesService locationImagesService,
            ILogger<LocationController> logger)
        {
            this.locationService = locationService;
            this.locationImagesService = locationImagesService;
            this.logger = logger;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ServiceResponse<ListViewModel<LocationDetailsViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(string filter = null, int skip = 0, int take = 9)
        {
            try
            {
                var response = await this.locationService.Filter(
                    filter, skip, take);

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

        [HttpGet("{locationId}")]
        [ProducesResponseType(typeof(ServiceResponse<LocationDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid locationId)
        {
            try
            {
                var response = await this.locationService.Get(locationId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }

                var locationImages = await this.locationImagesService.GetLocationImages(locationId);

                if (!locationImages.IsSuccess)
                {
                    return BadRequest(locationImages);
                }

                response.Data.LocationImages = locationImages.Data.LocationImages;

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
