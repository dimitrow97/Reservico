using Microsoft.EntityFrameworkCore;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.UserClients.Models;

namespace Reservico.Identity.UserClients
{
    public class UserClientManager : IUserClientManager
    {
        private readonly IRepository<Client> clientRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserClient> userClientRepository;

        public UserClientManager(
            IRepository<Client> clientRepository,
            IRepository<User> userRepository,
            IRepository<UserClient> userClientRepository)
        {
            this.clientRepository = clientRepository;
            this.userRepository = userRepository;
            this.userClientRepository = userClientRepository;
        }

        public async Task<ServiceResponse> AddUserToClient(
            AddUserToClientRequest request)
        {
            var user = await this.userRepository.Query()
                .Where(u => !u.IsDeleted)
                .Include(u => u.UserClients)
                .FirstOrDefaultAsync(x => x.Email.Equals(request.UserEmail));

            if (user is null)
            {
                return ServiceResponse.Error($"User not found.");
            }

            var client = await clientRepository.Query()
                .Where(c => !c.IsDeleted)
                .Include(c => c.UserClients)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.ClientId));

            if (client is null)
            {
                return ServiceResponse.Error($"Client not found.");
            }

            if (client.UserClients.Any(x => x.UserId.Equals(user.Id)))
            {
                return ServiceResponse.Error($"User is already linked to this client.");
            }

            var uc = new UserClient
            {
                ClientId = request.ClientId,
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                IsSelected = user.UserClients.Any() ? false : true
            };

            await this.userClientRepository.AddAsync(uc);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse<RemoveUserFromClientResponse>> RemoveUserFromClient(
            RemoveUserFromClientRequest request)
        {
            var uc = await this.userClientRepository.FindByConditionAsync(x =>
                x.UserId.Equals(request.UserId) && x.ClientId.Equals(request.ClientId));

            if (uc is null)
            {
                return ServiceResponse<RemoveUserFromClientResponse>
                    .Error($"User has no relation to this client.");
            }

            await this.userClientRepository.DeleteAsync(uc);

            var userClientsCount = await this.userClientRepository.Query()
                .Where(x => x.UserId.Equals(request.UserId))
                .CountAsync();

            return ServiceResponse<RemoveUserFromClientResponse>
                .Success(new RemoveUserFromClientResponse(userClientsCount));
        }

        public async Task<ServiceResponse<IEnumerable<UserClientsViewModel>>> GetUserClients(
            string userId)
        {
            var user = await this.userRepository.Query()
                .Where(u => !u.IsDeleted)
                .Include(u => u.UserClients)
                .ThenInclude(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id.Equals(userId));

            if (user is null)
            {
                return ServiceResponse<IEnumerable<UserClientsViewModel>>
                    .Error($"User not found.");
            }

            if (user.UserClients is null || !user.UserClients.Any())
            {
                return ServiceResponse<IEnumerable<UserClientsViewModel>>
                    .Error($"User is not linked to any clients.");
            }

            var userClients = user.UserClients.Select(
                x => new UserClientsViewModel
                {
                    ClientId = x.ClientId,
                    ClientName = x.Client.Name,
                    IsSelected = x.IsSelected
                });

            return ServiceResponse<IEnumerable<UserClientsViewModel>>
                .Success(userClients);
        }        

        public async Task<ServiceResponse> MarkClientAsSelected(
            MarkClientAsSelectedRequest request,
            string userId)
        {
            var uc = await this.userClientRepository.FindByConditionAsync(x =>
               x.UserId.Equals(userId) && x.ClientId.Equals(request.ClientId));

            if (uc is null)
            {
                return ServiceResponse.Error($"User has no relation to this client.");
            }

            var currentSelection = await this.userClientRepository.FindByConditionAsync(x =>
               x.UserId.Equals(userId) &&
               x.IsSelected);

            if (currentSelection != null)
            {
                currentSelection.IsSelected = false;
                await this.userClientRepository.UpdateAsync(currentSelection, false);
            }
            
            uc.IsSelected = true;
            
            await this.userClientRepository.UpdateAsync(uc, false);
            await this.userClientRepository.SaveChangesAsync();

            return ServiceResponse.Success();
        }
    }
}