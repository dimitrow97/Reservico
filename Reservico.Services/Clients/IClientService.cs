using Reservico.Common.Models;
using Reservico.Identity.UserManager.Models;
using Reservico.Services.Clients.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservico.Services.Clients
{
    public interface IClientService
    {
        Task<ServiceResponse<Guid>> CreateClient(CreateClientRequestModel model, string userId);

        Task<ServiceResponse<bool>> ClientExists(Guid clientId);

        Task<ServiceResponse> UpdateClient(UpdateClientRequestModel model, string userId);

        Task<ServiceResponse> DeleteClient(Guid clientId, string userId);

        Task<ServiceResponse<IEnumerable<ClientViewModel>>> GetAllClients();

        Task<ServiceResponse<ClientDetailsViewModel>> GetClient(Guid clientId);

        Task<ServiceResponse<string>> GetClientName(Guid clientId);

        Task<ServiceResponse<IEnumerable<UserViewModel>>> GetClientUsers(Guid clientId);
    }
}