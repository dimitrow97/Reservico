using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.UserPasswordManager;
using Reservico.Services.LocationImages.Models;
using Reservico.Services.Locations;

namespace Reservico.Services.LocationImages
{
    public class LocationImagesService : ILocationImagesService
    {
        private const string FILE_NAME = "reservico-{0}-{1}-{2}";

        private readonly ILocationService locationService;
        private readonly BlobConfiguration blobConfiguration;
        private readonly BlobServiceClient blobServiceClient;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly BlobContainerClient blobContainerClient;
        private readonly IRepository<LocationImage> locationImageRepository;

        public LocationImagesService(
            ILocationService locationService,
            BlobServiceClient blobServiceClient,
            IPasswordGenerator passwordGenerator,
            IRepository<LocationImage> locationImageRepository,
            IOptions<BlobConfiguration> configuration)
        {
            this.locationService = locationService;
            this.blobServiceClient = blobServiceClient;
            this.passwordGenerator = passwordGenerator;
            this.locationImageRepository = locationImageRepository;
            this.blobConfiguration = configuration.Value;

            this.blobContainerClient = this.blobServiceClient.GetBlobContainerClient(configuration.Value.ContainerName);
        }

        public async Task<ServiceResponse> UploadLocationImage(
            UploadLocationImageRequestModel model)
        {
            var location = await this.locationService
                .Get(model.LocationId);

            if (!location.IsSuccess)
            {
                return ServiceResponse.Error(
                    location.ErrorMessage);
            }

            Azure.Response<BlobContentInfo> response;

            string fileName = string.Format(
                FILE_NAME, 
                location.Data.Name, 
                this.passwordGenerator.GeneratePassword(8),
                model.File.FileName);

            using (var memoryStream = new MemoryStream())
            {
                model.File.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var clientResponse = await blobContainerClient
                    .UploadBlobAsync(fileName, memoryStream, default);
                response = clientResponse;
            }

            var locationImage = new LocationImage
            {
                BlobName = fileName,
                BlobPath = $"{blobConfiguration.ContainerName}/{fileName}",
                LocationId = model.LocationId,
                CreatedOn = DateTime.UtcNow,
            };

            await this.locationImageRepository.AddAsync(locationImage);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse<LocationImagesViewModel>> GetLocationImages(
            Guid locationId)
        {
            var locationExists = await this.locationService
                .LocationExists(locationId);

            if (!locationExists.IsSuccess)
            {
                return ServiceResponse<LocationImagesViewModel>
                    .Error(locationExists.ErrorMessage);
            }

            var locationImages = await this.locationImageRepository
                .Query()
                .Where(x => !x.IsDeleted)
                .Where(x => x.LocationId.Equals(locationId))
                .ToListAsync();

            var result = new LocationImagesViewModel
            {
                LocationId = locationId,
                LocationImages = locationImages.Select(x => new LocationImageViewModel
                {
                    LocationImageId = x.Id,
                    BlobPath = x.BlobPath
                })
            };

            return ServiceResponse<LocationImagesViewModel>
                .Success(result);
        }
    }
}
