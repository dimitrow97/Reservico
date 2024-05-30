using Reservico.Common.Models;
using Reservico.Services.Locations.Models;

namespace Reservico.Services.Locations
{
    public interface ILocationService
    {
        Task<ServiceResponse> Create(CreateLocationRequestModel model);

        Task<ServiceResponse<LocationDetailsViewModel>> Get(
            Guid locationId);

        Task<ServiceResponse<IEnumerable<LocationViewModel>>> GetLocations(Guid clientId);

        Task<ServiceResponse<IEnumerable<TableViewModel>>> GetLocationTables(
            Guid locationId);

        Task<ServiceResponse<TableViewModel>> GetTable(
            Guid tableId);

        Task<ServiceResponse> LocationExists(Guid locationId);

        Task<ServiceResponse> Update(UpdateLocationRequestModel model);

        Task<ServiceResponse> Delete(Guid locationId);

        Task<ServiceResponse> AddTable(AddTableRequestModel model);

        Task<ServiceResponse> TableExists(Guid tableId);

        Task<ServiceResponse> UpdateTable(UpdateTableRequestModel model);

        Task<ServiceResponse> DeleteTable(Guid tableId);

        Task<ServiceResponse<ListViewModel<LocationDetailsViewModel>>> Filter(
            string filter = null,
            int skip = 0,
            int take = 9);
    }
}