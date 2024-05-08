using Reservico.Common.Models;
using Reservico.Identity.UserPasswordManager.Models;

namespace Reservico.Identity.UserPasswordManager
{
    public interface IUserPasswordManager
    {
        Task<ServiceResponse<ValidateUserCredentialsResponseModel>> ValidateUserCredentialsAsync(
            ValidateUserCredentialsRequestModel model);

        Task<ServiceResponse> ResetPasswordAsync(string email);

        Task<ServiceResponse> ResetPasswordAsync(ResetPasswordRequestModel model);

        Task<ServiceResponse> ChangePasswordAsync(ChangePasswordRequestModel model, Guid userId);
    }
}