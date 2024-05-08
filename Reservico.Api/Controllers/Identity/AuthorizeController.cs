using Reservico.Api.Controllers.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserPasswordManager;
using Reservico.Identity.Code;
using Reservico.Data.Entities;
using Reservico.Identity.IdentityConfig;
using Reservico.Identity.Constants;
using Reservico.Identity.UserPasswordManager.Models;
using Reservico.Identity.Code.Models;

namespace Reservico.Api.Controllers.Identity
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly ILogger<AuthorizeController> logger;
        private readonly IUserPasswordManager userPasswordManager;
        private readonly ICodeProvider<IdentityOneTimeCode> oneTimeCodeProvider;
        private readonly ICodeProvider<IdentityAuthorizationCode> authorizationCodeProvider;
        private readonly IIdentityAuthorizationConfigProvider identityAuthorizationConfigProvider;

        public AuthorizeController(
            IUserManager userManager,
            ILogger<AuthorizeController> logger,
            IUserPasswordManager userPasswordManager,
            ICodeProvider<IdentityOneTimeCode> oneTimeCodeProvider,
            ICodeProvider<IdentityAuthorizationCode> authorizationCodeProvider,
            IIdentityAuthorizationConfigProvider identityAuthorizationConfigProvider)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.userPasswordManager = userPasswordManager;
            this.oneTimeCodeProvider = oneTimeCodeProvider;
            this.authorizationCodeProvider = authorizationCodeProvider;
            this.identityAuthorizationConfigProvider = identityAuthorizationConfigProvider;
        }

        /// <summary>
        /// Initiates user log in process.
        /// </summary>
        /// <param name="model">Authentication Request Model.</param>
        /// <returns>Model containing either authorization code or one-time use code for OTP.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthorizeLoginRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            if (!model.GrantType.Equals(IdentityAuthorizationGrantTypes.AuthorizationCode))
            {
                return this.BadRequest("Invalid grant type.");
            }

            var validationResult = this.identityAuthorizationConfigProvider
                .ValidateClientConfig(model.ClientId);

            if (!validationResult.IsSuccess)
            {
                return this.BadRequest(validationResult.ErrorMessage);
            }

            try
            {
                var credentialsCheckResult = await this.userPasswordManager.ValidateUserCredentialsAsync(
                    new ValidateUserCredentialsRequestModel(model.Email, model.Password));

                if (!credentialsCheckResult.IsSuccess)
                {
                    return this.BadRequest("Invalid user credentials.");
                }

                var userAccessChcek = this.identityAuthorizationConfigProvider.ValidateUserAccess(
                    model.ClientId, credentialsCheckResult.Data.UserRoles);

                if (!userAccessChcek.IsSuccess)
                {
                    return this.BadRequest("Invalid user credentials.");
                }

                var config = this.identityAuthorizationConfigProvider.GetConfiguration();

                    return await this.GenerateAuthorizationCode(
                        model.ClientId,
                        config.AuthorizationCodeExpirationMinutes,
                        credentialsCheckResult.Data.UserId,
                        credentialsCheckResult.Data.UserSecurityStamp);                
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        
        private async Task<IActionResult> GenerateAuthorizationCode(
            string clientId,
            int authCodeExpirationMinutes,
            Guid userId,
            string userSecurityStamp)
        {
            var authorizationCode = await authorizationCodeProvider.GenerateCodeAsync(
                    new GenerateCodeRequestModel(
                        clientId,
                        authCodeExpirationMinutes,
                        ExpirationTimeType.Minutes,
                        userId.ToString(),
                        userSecurityStamp));

            return this.Ok(AuthorizeLoginResponseModel.AuthCode(authorizationCode));
        }
    }
}