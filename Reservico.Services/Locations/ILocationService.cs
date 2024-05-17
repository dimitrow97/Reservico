using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Services.Locations.Models;

namespace Reservico.Services.Locations
{
    public interface ILocationService
    {
        Task<ServiceResponse> Create(CreateLocationRequestModel model);

        Task<ServiceResponse<LocationViewModel>> Get(Guid locationId);

        Task<ServiceResponse<IEnumerable<Table>>> GetLocationTables(Guid locationId);

        Task<ServiceResponse> LocationExists(Guid locationId);

        Task<ServiceResponse> Update(UpdateLocationRequestModel model);

        Task<ServiceResponse> Delete(Guid locationId);

        Task<ServiceResponse> AddTable(AddTableRequestModel model);

        Task<ServiceResponse> TableExists(Guid tableId);

        Task<ServiceResponse> UpdateTable(UpdateTableRequestModel model);

        Task<ServiceResponse> DeleteTable(Guid tableId);
    }
}