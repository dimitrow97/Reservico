using Reservico.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Reservico.Data.Entities;
using Reservico.Common.Models;
using Reservico.Services.Clients.Models;
using Reservico.Identity.UserManager.Models;

namespace Reservico.Services.Clients
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> clientRepository;

        public ClientService(
            IRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task<ServiceResponse<bool>> ClientExists(Guid clientId)
        {
            var result = await clientRepository.Query()
                .Where(c => !c.IsDeleted).AnyAsync(x => x.Id == clientId);

            return ServiceResponse<bool>.Success(result);
        }

        public async Task<ServiceResponse<Guid>> CreateClient(CreateClientRequestModel model, string userId)
        {
            var clientId = Guid.NewGuid();

            var client = new Client
            {
                Id = clientId,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Name = model.Name,
                Postcode = model.Postcode,
                CreatedOn = DateTime.UtcNow,
            };                       

            await clientRepository.AddAsync(client);

            return ServiceResponse<Guid>.Success(client.Id);
        }

        public async Task<ServiceResponse> UpdateClient(UpdateClientRequestModel model, string userId)
        {
            var client = await clientRepository
                .Query()
                .Where(x => !x.IsDeleted)
                .Where(c => c.Id == model.Id)
                .FirstOrDefaultAsync();

            if (client is null)
            {
                return ServiceResponse
                    .Error($"No client found with Id: {model.Id}");
            }            

            client.Name = model.Name;
            client.Address = model.Address;
            client.City = model.City;
            client.Postcode = model.Postcode;
            client.Country = model.Country;
            client.UpdatedOn = DateTime.UtcNow;            

            await clientRepository.UpdateAsync(client);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> DeleteClient(Guid clientId, string userId)
        {
            var client = await clientRepository
                .Query()
                .FirstOrDefaultAsync(c => c.Id.Equals(clientId));

            if (client is null)
            {
                return ServiceResponse
                    .Error($"No client found with Id: {clientId}.");
            }

            if (client.IsDeleted)
            {
                return ServiceResponse
                    .Error($"Client with Id: {clientId}, is already deleted.");
            }

            client.IsDeleted = true;
            client.UpdatedOn = DateTime.UtcNow;

            await clientRepository.UpdateAsync(client);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse<IEnumerable<ClientViewModel>>> GetAllClients()
        {
            var clientEntities = await clientRepository.Query()
                .Where(c => !c.IsDeleted)                
                .ToListAsync();

            if (clientEntities is null || !clientEntities.Any())
            {
                return ServiceResponse<IEnumerable<ClientViewModel>>
                    .Error("No clients present in the database.");
            }

            var clientViewModels = new List<ClientViewModel>();

            foreach (var client in clientEntities)
            {
                var vm = this.GetClientViewModelAsync(client);

                clientViewModels.Add(vm);
            }

            return ServiceResponse<IEnumerable<ClientViewModel>>
                .Success(clientViewModels);
        }

        public async Task<ServiceResponse<IEnumerable<UserViewModel>>> GetClientUsers(Guid clientId)
        {
            var client = await clientRepository.Query()
                .Where(c => !c.IsDeleted)
                .Include(x => x.UserClients)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id.Equals(clientId));

            if (client is null)
            {
                return ServiceResponse<IEnumerable<UserViewModel>>.Error($"No client found with Id: {clientId}");
            }

            var users = client.UserClients
                .Where(x => !x.User.IsDeleted)
                .Select(uc => new UserViewModel
                {
                    UserId = uc.User.Id.ToString(),
                    Email = uc.User.Email,
                    FullName = $"{uc.User.FirstName} {uc.User.LastName}",
                    IsActive = uc.User.IsActive ?? false,
                    IsUsingDefaultPassword = uc.User.IsUsingDefaultPassword ?? false
                });

            return ServiceResponse<IEnumerable<UserViewModel>>.Success(users);
        }

        public async Task<ServiceResponse<ClientDetailsViewModel>> GetClient(Guid clientId)
        {
            var client = await clientRepository.Query()                
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id.Equals(clientId));
            
            if (client is null)
            {
                return ServiceResponse<ClientDetailsViewModel>
                    .Error($"No client found with Id: {clientId}");
            }           

            var clientVm = new ClientDetailsViewModel
            {
                DateCreated = client.CreatedOn,
                Id = client.Id.ToString(),
                Name = client.Name,
                Address = client.Address,
                City = client.City,
                Postcode = client.Postcode,
                Country = client.Country                
            };

            return ServiceResponse<ClientDetailsViewModel>
                .Success(clientVm);
        }

        public async Task<ServiceResponse<string>> GetClientName(Guid clientId)
        {
            var client = await clientRepository.Query()
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id.Equals(clientId));

            if (client is null)
            {
                return ServiceResponse<string>
                    .Error($"No client found with Id: {clientId}");
            }

            return ServiceResponse<string>.Success(client.Name);
        }        

        private ClientViewModel GetClientViewModelAsync(Client client)
        {
            var clientVm = new ClientViewModel
            {
                DateCreated = client.CreatedOn,
                Id = client.Id.ToString(),
                Name = client.Name,
            };

            return clientVm;
        } 
    }
}