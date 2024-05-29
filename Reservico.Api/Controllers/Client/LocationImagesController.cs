using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Identity.UserManager;
using Reservico.Services.Locations.Models;
using Reservico.Services.Locations;
using Reservico.Services.LocationImages;
using Reservico.Services.LocationImages.Models;

namespace Reservico.Api.Controllers.Client
{
    public class LocationImagesController : BaseClientController
    {
        private readonly ILocationImagesService locationImagesService;
        private readonly ILogger<LocationImagesController> logger;

        public LocationImagesController(
            ILocationImagesService locationImagesService,
            ILogger<LocationImagesController> logger)
        {
            this.locationImagesService = locationImagesService;
            this.logger = logger;
        }

        [HttpGet("{locationId}")]
        [ProducesResponseType(typeof(ServiceResponse<LocationImagesViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<LocationImagesViewModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid locationId)
        {
            try
            {
                var response = await this.locationImagesService
                    .GetLocationImages(locationId);

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

        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadImage([FromForm]UploadLocationImageRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {         
                var response = await this.locationImagesService
                    .UploadLocationImage(model);

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

        [HttpDelete]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLocationImage(Guid locationImageId)
        {
            try
            {
                var response = await this.locationImagesService
                    .DeleteLocationImages(locationImageId);

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
