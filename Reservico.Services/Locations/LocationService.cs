using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.UserManager.Models;
using Reservico.Identity.UserManager;
using Reservico.Services.Categories;
using Reservico.Services.Clients;
using Reservico.Services.Locations.Models;
using Reservico.Services.Reservations.Models;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core;
using Reservico.Services.LocationImages.Models;

namespace Reservico.Services.Locations
{
    public class LocationService : ILocationService
    {
        private readonly IMapper mapper;
        private readonly IClientService clientService;
        private readonly ICategoryService categoryService;
        private readonly IRepository<Table> tableRepository;
        private readonly IRepository<Location> locationRepository;
        private readonly IRepository<Reservation> reservationRepository;

        public LocationService(
            IMapper mapper,
            IClientService clientService,
            ICategoryService categoryService,
            IRepository<Table> tableRepository,
            IRepository<Location> locationRepository,
            IRepository<Reservation> reservationRepository)
        {
            this.mapper = mapper;
            this.clientService = clientService;
            this.categoryService = categoryService;
            this.tableRepository = tableRepository;
            this.locationRepository = locationRepository;
            this.reservationRepository = reservationRepository;
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
                Email = model.Email,
                Address = model.Address,
                City = model.City,
                Postcode = model.Postcode,
                Country = model.Country,
                ClientId = model.ClientId,
                CreatedOn = DateTime.UtcNow
            };

            await this.locationRepository.AddAsync(location);

            foreach (var category in model.Categories)
            {
                var categoryExists = await this.categoryService
                    .CategoryExists(category);

                var lc = new LocationCategory
                {
                    Location = location,
                    CategoryId = category
                };

                location.LocationCategories.Add(lc);
            }

            await this.locationRepository.UpdateAsync(location);

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

        public async Task<ServiceResponse<LocationDetailsViewModel>> Get(Guid locationId)
        {
            var location = await this.locationRepository.Query()
                .Include(x => x.Client)
                .Include(x => x.Tables)
                .Include(x => x.LocationCategories)
                    .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id.Equals(locationId) && !x.IsDeleted);

            if (location is null)
            {
                return ServiceResponse<LocationDetailsViewModel>.
                    Error("Location does NOT exists");
            }

            var result = this.mapper.Map(location, new LocationDetailsViewModel());

            if (location.Tables != null && location.Tables.Any()) 
            {
                result.WorkingHoursFrom = location.Tables.Min(x => x.WorkingHoursFrom);
                result.WorkingHoursTo = location.Tables.Max(x => x.WorkingHoursTo);
            }

            return ServiceResponse<LocationDetailsViewModel>.Success(result);
        }

        public async Task<ServiceResponse<IEnumerable<LocationViewModel>>> GetLocations(
            Guid clientId)
        {
            var clientExists = await this.clientService.ClientExists(clientId);

            if (!clientExists.IsSuccess)
            {
                return ServiceResponse<IEnumerable<LocationViewModel>>
                    .Error(clientExists.ErrorMessage);
            }

            var locations = await this.locationRepository.Query()
                .Include(x => x.Client)
                .Include(x => x.LocationCategories)
                    .ThenInclude(x => x.Category)
                .Where(x => x.ClientId.Equals(clientId) && !x.IsDeleted)
                .ToListAsync();

            if (locations is null)
            {
                return ServiceResponse<IEnumerable<LocationViewModel>>
                    .Error("No Locations for the provided ClientId");
            }

            return ServiceResponse<IEnumerable<LocationViewModel>>
                .Success(locations.Select(x => this.mapper.Map(x, new LocationViewModel())));
        }

        public async Task<ServiceResponse<IEnumerable<TableViewModel>>> GetLocationTables(
            Guid locationId)
        {
            var locationExists = await this.LocationExists(locationId);

            if (!locationExists.IsSuccess)
            {
                return ServiceResponse<IEnumerable<TableViewModel>>.Error(
                    locationExists.ErrorMessage);
            }

            var location = await this.locationRepository.Query()
                .Include(x => x.Tables)
                .FirstOrDefaultAsync(x => x.Id.Equals(locationId));

            return ServiceResponse<IEnumerable<TableViewModel>>.Success(
                location.Tables.Where(x => !x.IsDeleted)
                .Select(x => this.mapper.Map(x, new TableViewModel())));
        }

        public async Task<ServiceResponse<TableViewModel>> GetTable(
            Guid tableId)
        {
            var tableExists = await this.TableExists(tableId);

            if (!tableExists.IsSuccess)
            {
                return ServiceResponse<TableViewModel>
                    .Error(tableExists.ErrorMessage);
            }

            var table = await this.tableRepository.Query()
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id.Equals(tableId) && !x.IsDeleted);

            var result = this.mapper.Map(table, new TableViewModel());

            return ServiceResponse<TableViewModel>.Success(result);
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

            var location = await this.locationRepository
                .Query()
                .Include(x => x.LocationCategories)
                .FirstOrDefaultAsync(x => x.Id.Equals(model.LocationId));

            location.Name = model.Name;
            location.Email = model.Email;
            location.Address = model.Address;
            location.City = model.City;
            location.Postcode = model.Postcode;
            location.Country = model.Country;

            await this.HandleCategoriesOnUpdate(
                location, model.Categories);

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

        public async Task<ServiceResponse<ListViewModel<LocationDetailsViewModel>>> Filter(
            string filter = null,
            int skip = 0,
            int take = 9)
        {
            var queryable = this.locationRepository.Query()
                .Include(x => x.Tables)
                .Include(x => x.Client)
                .Include(x => x.LocationImages)
                .Include(x => x.LocationCategories)
                    .ThenInclude(x => x.Category)
                .Where(x => !x.IsDeleted);

            var result = await this.GetInner(queryable, filter, skip, take);

            if (!result.IsSuccess)
            {
                return ServiceResponse<ListViewModel<LocationDetailsViewModel>>
                    .Error(result.ErrorMessage);
            }

            return result;
        }

        private async Task<ServiceResponse<ListViewModel<LocationDetailsViewModel>>> GetInner(
            IQueryable<Location> queryable,
            string filter = null,
            int skip = 0,
            int take = 10)
        {
            //filtering
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = RenameQueryFields(filter);

                queryable = queryable.Where(filter);
            }

            var totalCount = await queryable.CountAsync();

            //paging
            queryable = queryable.Skip(skip).Take(take);

            var locations = await queryable
                .ToListAsync();

            var result = locations.Select(location => mapper.Map(location, new LocationDetailsViewModel
            {
                WorkingHoursFrom = location.Tables.Min(x => x.WorkingHoursFrom),
                WorkingHoursTo = location.Tables.Max(x => x.WorkingHoursTo),
                LocationImages = location.LocationImages.Where(x => !x.IsDeleted).Select(x => new LocationImageViewModel
                {
                    LocationImageId = x.Id,
                    BlobPath = x.BlobPath
                })
            }));

            return ServiceResponse<ListViewModel<LocationDetailsViewModel>>.Success(
                new ListViewModel<LocationDetailsViewModel>
                {
                    TotalCount = totalCount,
                    NumberOfPages = totalCount > take ? (int)Math.Ceiling((double)totalCount / take) : 1,
                    Data = result
                });
        }

        private string RenameQueryFields(string query)
        {
            var result = query.ToLower()
               .Replace("postcode", "Postcode")
               .Replace("city", "City")
               .Replace("country", "Country")
               .Replace("name", "Name");

            var rgx = new Regex("category.contains\\((.+)\\)");

            if (rgx.IsMatch(result))
            {
                var match = rgx.Match(result);
                match.Groups.TryGetValue("1", out var categoryNameFilter);

                result = rgx.Replace(result, $"LocationCategories.Any(y => y.Category.Name.Contains({categoryNameFilter.Value}))");
            }

            return result;
        }

        private async Task HandleCategoriesOnUpdate(
            Location location,
            IList<Guid> categories)
        {
            var categoriesAll = await this.categoryService
                .GetAll();

            var categoriesInDb = categoriesAll.Data
                .Where(r => categories.Any(x => r.CategoryId.Equals(x)));

            if (location.LocationCategories is null || !location.LocationCategories.Any())
            {
                foreach (var dbCategory in categoriesInDb)
                {
                    location.LocationCategories.Add(new LocationCategory
                    {
                        CategoryId = dbCategory.CategoryId,
                        LocationId = location.Id
                    });
                }
            }
            else
            {
                foreach (var c in location.LocationCategories)
                {
                    if (categories.Any(x => x.Equals(c.CategoryId)))
                    {
                        categories.Remove(c.CategoryId);
                    }
                    else
                    {
                        location.LocationCategories.Remove(c);
                    }
                }

                if (categories.Any())
                {
                    foreach (var c in categoriesInDb.Where(x => categories.Any(r => x.CategoryId.Equals(r))))
                    {
                        location.LocationCategories.Add(
                            new LocationCategory
                            {
                                CategoryId = c.CategoryId,
                                LocationId = location.Id
                            });
                    }
                }
            }
        }
    }
}
