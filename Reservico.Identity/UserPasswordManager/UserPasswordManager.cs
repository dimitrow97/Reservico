using Microsoft.EntityFrameworkCore;
using Reservico.Common.EmailSender;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserPasswordManager.Models;
using System.Text;

namespace Reservico.Identity.UserPasswordManager
{
    public class UserPasswordManager : IUserPasswordManager
    {
        private readonly IUserManager userManager;
        private readonly IEmailSender emailSender;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRepository<Client> clientRepository;
        private readonly IRepository<UserClient> userClientRepository;

        public UserPasswordManager(
            IUserManager userManager,
            IEmailSender emailSender,
            IPasswordHasher passwordHasher,
            IRepository<Client> clientRepository,
            IRepository<UserClient> userClientRepository)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.passwordHasher = passwordHasher;
            this.clientRepository = clientRepository;
            this.userClientRepository = userClientRepository;
        }

        public async Task<ServiceResponse<ValidateUserCredentialsResponseModel>> ValidateUserCredentialsAsync(
            ValidateUserCredentialsRequestModel model)
        {
            var user = await userManager.GetUserByEmailAsync<User>(model.Email);
            var userClients = !user.IsSuccess ? null : await userClientRepository
                .Query()
                .Include(x => x.Client)
                .Where(x => x.UserId.Equals(user.Data.Id) && !x.Client.IsDeleted)
                .ToListAsync();

            if (!user.IsSuccess ||
                user.Data.IsDeleted ||
                userClients?.Any() != true &&
                !await this.IsAdminUser(user.Data))
            {
                return ServiceResponse<ValidateUserCredentialsResponseModel>
                    .Error("No user matches the provided email.");
            }

            if (user.Data.IsActive.HasValue && !user.Data.IsActive.Value)
            {
                return ServiceResponse<ValidateUserCredentialsResponseModel>
                    .Error("User is suspended.");
            }

            var userRoles = await this.userManager.GetUserRoles(
                user.Data.Id);

            if (!userRoles.IsSuccess)
            {
                return ServiceResponse<ValidateUserCredentialsResponseModel>
                    .Error(userRoles.ErrorMessage);
            }

            var result = this.VerifyPassword(user.Data, model.Password);

            if (!result)
            {
                return ServiceResponse<ValidateUserCredentialsResponseModel>
                    .Error("The provided password is invalid.");
            }

            return ServiceResponse<ValidateUserCredentialsResponseModel>
                    .Success(new ValidateUserCredentialsResponseModel(
                        user.Data.Id,
                        user.Data.SecurityStamp,
                        user.Data.PhoneNumber,
                        user.Data.IsUsingDefaultPassword ?? false,
                        userRoles.Data));
        }

        public async Task<ServiceResponse> ResetPasswordAsync(string email)
        {
            var user = await userManager.GetUserByEmailAsync<User>(email);

            if (!user.IsSuccess)
            {
                return ServiceResponse.Error("User not found.");
            }

            var passwordResetToken = this.GeneratePasswordResetToken(user.Data.SecurityStamp);

            if (string.IsNullOrWhiteSpace(passwordResetToken))
            {
                return ServiceResponse.Error("Error occurred in resetting password.");
            }

            await emailSender.SendPasswordResetEmail(
                user.Data.Email,
                passwordResetToken);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> ResetPasswordAsync(ResetPasswordRequestModel model)
        {
            var user = await userManager.GetUserByEmailAsync<User>(model.Email);

            if (!user.IsSuccess)
            {
                return ServiceResponse.Error("User not found.");
            }

            var result = this.VerifyPasswordResetToken(model.Token, user.Data.SecurityStamp);

            if (!result)
            {
                return ServiceResponse.Error("Invalid password reset token has been provided.");
            }

            user.Data.SecurityStamp = Guid.NewGuid().ToString();
            user.Data.PasswordHash = this.passwordHasher.Hash(model.Password, user.Data.SecurityStamp);

            if (!user.Data.IsUsingDefaultPassword.HasValue || user.Data.IsUsingDefaultPassword.Value)
            {
                user.Data.IsUsingDefaultPassword = false;
            }

            await userManager.UpdateAsync(user.Data);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> ChangePasswordAsync(
            ChangePasswordRequestModel model,
            Guid userId)
        {
            var user = await userManager.GetUserByIdAsync<User>(userId);

            if (!user.IsSuccess)
            {
                return ServiceResponse.Error("User not found.");
            }

            var result = this.VerifyPassword(user.Data, model.Password);

            if (!result)
            {
                return ServiceResponse<ValidateUserCredentialsResponseModel>
                    .Error("The provided old password is invalid.");
            }

            user.Data.SecurityStamp = Guid.NewGuid().ToString();
            user.Data.PasswordHash = this.passwordHasher.Hash(model.NewPassword, user.Data.SecurityStamp);
            

            if (!user.Data.IsUsingDefaultPassword.HasValue || user.Data.IsUsingDefaultPassword.Value)
            {
                user.Data.IsUsingDefaultPassword = false;
            }

            await userManager.UpdateAsync(user.Data);

            return ServiceResponse.Success();
        }

        private async Task<bool> IsAdminUser(
            User user)
        {
            var roles = await userManager.GetUserRoles(user.Id);

            if (!roles.IsSuccess || !roles.Data.Any())
            {
                return false;
            }

            return roles.Data.Contains(IdentityRoles.AdminRole);
        }        

        private bool VerifyPassword(
            User user, string userInput)        
            => this.passwordHasher.Verify(
                userInput,
                user.SecurityStamp,
                user.PasswordHash);

        private string GeneratePasswordResetToken(string securityStamp)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(securityStamp));

        private bool VerifyPasswordResetToken(string token, string securityStamp)
            => token.Equals(this.GeneratePasswordResetToken(securityStamp));
    }
}