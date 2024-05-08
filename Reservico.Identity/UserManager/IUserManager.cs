using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Identity.UserManager.Models;

namespace Reservico.Identity.UserManager
{
    public interface IUserManager
    {
        Task<ServiceResponse<UserRegistrationResponseModel>> RegisterAsync(
            UserRegistrationRequestModel model);

        Task<ServiceResponse> UpdateAsync(
            UserDetailsViewModel model);

        Task<ServiceResponse> UpdateAsync(
            User model);

        Task<ServiceResponse> DeleteAsync(
            Guid userId);

        Task<ServiceResponse<TModel>> GetUserByIdAsync<TModel>(
            Guid userId) where TModel : class, new();

        Task<ServiceResponse<TModel>> GetUserByEmailAsync<TModel>(
            string email) where TModel : class, new();

        Task<ServiceResponse<IEnumerable<TModel>>> GetUsersByRoleAsync<TModel>(
            string role) where TModel : class, new();

        Task<ServiceResponse> VerifyUserPhoneNumberAsync(
            Guid userId);

        Task<ServiceResponse<IEnumerable<string>>> GetUserRoles(
            Guid userId);

        Task<ServiceResponse<Client>> GetUserSelectedClient(
            Guid userId);
    }
}
