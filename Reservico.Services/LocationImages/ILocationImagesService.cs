using Microsoft.AspNetCore.Http;
using Reservico.Common.Models;
using Reservico.Services.LocationImages.Models;

namespace Reservico.Services.LocationImages
{
    public interface ILocationImagesService
    {
        Task<ServiceResponse> UploadLocationImage(
            UploadLocationImageRequestModel model);

        Task<ServiceResponse<LocationImagesViewModel>> GetLocationImages(
            Guid locationId);
    }
}
