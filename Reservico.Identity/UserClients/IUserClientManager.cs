using Reservico.Common.Models;
using Reservico.Identity.UserClients.Models;

namespace Reservico.Identity.UserClients
{
    public interface IUserClientManager
    {
        Task<ServiceResponse> AddUserToClient(
            AddUserToClientRequest request);

        Task<ServiceResponse<RemoveUserFromClientResponse>> RemoveUserFromClient(
            RemoveUserFromClientRequest request);

        Task<ServiceResponse<IEnumerable<UserClientsViewModel>>> GetUserClients(
            string userId);

        Task<ServiceResponse> MarkClientAsSelected(
            MarkClientAsSelectedRequest request,
            string userId);
    }
}
