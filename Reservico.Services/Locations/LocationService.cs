using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Services.Clients;
using Reservico.Services.Locations.Models;
using System.Collections.Generic;

namespace Reservico.Services.Locations
{
    public class LocationService : ILocationService
    {
        private readonly IMapper mapper;
        private readonly IClientService clientService;
        private readonly IRepository<Table> tableRepository;
        private readonly IRepository<Location> locationRepository;

        public LocationService(
            IMapper mapper,
            IClientService clientService,
            IRepository<Table> tableRepository,
            IRepository<Location> locationRepository)
        {
            this.mapper = mapper;
            this.clientService = clientService;
            this.tableRepository = tableRepository;
            this.locationRepository = locationRepository;
        }

        public async Task<ServiceResponse> Create(CreateLocationRequestModel model)
        {
            var clientExists = await this.clientService.ClientExists(model.ClientId);

            if (!clientExists.IsSuccess)
            {
                return ServiceResponse.Error(
                    clientExists.ErrorMessage);
            }

            var locationExists = await this.locationRepository.Query()
                .Where(c => !c.IsDeleted)
                .AnyAsync(x => x.Name.Equals(model.Name) && x.Address.Equals(model.Address) && x.City.Equals(model.City));

            if (locationExists)
            {
                return ServiceResponse.Error(
                    $"Location with name: {model.Name}, address: {model.Address}, {model.City} already exists");
            }

            var location = new Location
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Postcode = model.Postcode,
                Country = model.Country,
                ClientId = model.ClientId,
                CreatedOn = DateTime.UtcNow
            };

            await this.locationRepository.AddAsync(location);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> LocationExists(Guid locationId)
        {
            var result = await locationRepository.Query()
                .Where(c => !c.IsDeleted).AnyAsync(x => x.Id == locationId);

            if (result == false)
            {
                return ServiceResponse.Error("Location does NOT exist");
            }

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse<LocationViewModel>> Get(Guid locationId)
        {
            var location = await this.locationRepository.Query()
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id.Equals(locationId) && !x.IsDeleted);

            if (location is null)
            {
                return ServiceResponse<LocationViewModel>.
                    Error("Location does NOT exists");
            }

            var result = this.mapper.Map(location, new LocationViewModel());

            return ServiceResponse<LocationViewModel>.Success(result);
        }

        public async Task<ServiceResponse<IEnumerable<Table>>> GetLocationTables(
            Guid locationId)
        {
            var locationExists = await this.LocationExists(locationId);

            if (!locationExists.IsSuccess)
            {
                return ServiceResponse<IEnumerable<Table>>.Error(
                    locationExists.ErrorMessage);
            }

            var location = await this.locationRepository.Query()
                .Include(x => x.Tables)
                    .ThenInclude(x => x.Reservations)
                .FirstOrDefaultAsync(x => x.Id.Equals(locationId));

            return ServiceResponse<IEnumerable<Table>>.Success(
                location.Tables.Where(x => !x.IsDeleted));
        }

        public async Task<ServiceResponse> Update(UpdateLocationRequestModel model)
        {
            var locationExists = await this.LocationExists(
                model.LocationId);

            if (!locationExists.IsSuccess)
            {
                return ServiceResponse.Error(
                    locationExists.ErrorMessage);
            }

            var location = await this.locationRepository.GetByIdAsync(model.LocationId);

            location.Name = model.Name;
            location.Address = model.Address;
            location.City = model.City;
            location.Postcode = model.Postcode;
            location.Country = model.Country;

            await this.locationRepository.SaveChangesAsync();

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> Delete(Guid locationId)
        {
            var locationExists = await this.LocationExists(
               locationId);

            if (!locationExists.IsSuccess)
            {
                return ServiceResponse.Error(
                    locationExists.ErrorMessage);
            }

            var location = await this.locationRepository.GetByIdAsync(locationId);

            location.IsDeleted = true;
            location.UpdatedOn = DateTime.UtcNow;

            await this.locationRepository.SaveChangesAsync();

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> AddTable(AddTableRequestModel model)
        {
            var locationExists = await this.LocationExists(
                 model.LocationId);

            if (!locationExists.IsSuccess)
            {
                return ServiceResponse.Error(
                    locationExists.ErrorMessage);
            }

            var location = await this.locationRepository.GetByIdAsync(model.LocationId);

            var table = new Table
            {
                Name = model.Name,
                Capacity = model.Capacity,
                Description = model.Description,
                WorkingHoursFrom = model.WorkingHoursFrom,
                WorkingHoursTo = model.WorkingHoursTo,
                CanTableTurn = model.CanTableTurn,
                TableTurnOffset = model.TableTurnOffset,
                Location = location,
                CreatedOn = DateTime.UtcNow
            };

            await this.tableRepository.AddAsync(table);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> TableExists(Guid tableId)
        {
            var result = await tableRepository.Query()
                .Where(c => !c.IsDeleted).AnyAsync(x => x.Id == tableId);

            if (result == false)
            {
                return ServiceResponse.Error("Table does NOT exist");
            }

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> UpdateTable(UpdateTableRequestModel model)
        {
            var table = await this.tableRepository.Query()
                .FirstOrDefaultAsync(x => x.Id.Equals(model.TableId) && !x.IsDeleted);

            if (table is null)
            {
                return ServiceResponse.Error(
                    "Table does NOT exist");
            }

            table.Name = model.Name;
            table.Capacity = model.Capacity;
            table.Description = model.Description;
            table.WorkingHoursFrom = model.WorkingHoursFrom;
            table.WorkingHoursTo = model.WorkingHoursTo;
            table.CanTableTurn = model.CanTableTurn;
            table.TableTurnOffset = model.TableTurnOffset;
            table.UpdatedOn = DateTime.UtcNow;

            await this.tableRepository.UpdateAsync(table);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> DeleteTable(Guid tableId)
        {
            var table = await this.tableRepository.Query()
                .FirstOrDefaultAsync(x => x.Id.Equals(tableId) && !x.IsDeleted);

            if (table is null)
            {
                return ServiceResponse.Error(
                    "Table does NOT exist");
            }
            table.IsDeleted = true;
            table.UpdatedOn = DateTime.UtcNow;

            await this.tableRepository.UpdateAsync(table);

            return ServiceResponse.Success();
        }
    }
}
