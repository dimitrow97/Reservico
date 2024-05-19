using Microsoft.AspNetCore.Mvc;
using Reservico.Api.Controllers.Identity.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.Code;
using Reservico.Identity.Constants;
using Reservico.Identity.IdentityConfig;
using Reservico.Identity.Token;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserManager.Models;

namespace Reservico.Api.Controllers.Identity
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly ITokenProvider tokenProvider;
        private readonly ILogger<TokenController> logger;
        private readonly IRepository<UserClient> userClientRepository;
        private readonly ICodeProvider<IdentityAuthorizationCode> authorizationCodeProvider;
        private readonly IIdentityAuthorizationConfigProvider identityAuthorizationConfigProvider;

        public TokenController(
            IUserManager userManager,
            ITokenProvider tokenProvider,
            ILogger<TokenController> logger,
            IRepository<UserClient> userClientRepository,
            ICodeProvider<IdentityAuthorizationCode> authorizationCodeProvider,
            IIdentityAuthorizationConfigProvider identityAuthorizationConfigProvider)
        {
            this.userManager = userManager;
            this.tokenProvider = tokenProvider;
            this.logger = logger;
            this.userClientRepository = userClientRepository;
            this.authorizationCodeProvider = authorizationCodeProvider;
            this.identityAuthorizationConfigProvider = identityAuthorizationConfigProvider;
        }

        /// <summary>
        /// Generates authentication tokens based on provided authorization code.
        /// </summary>
        /// <param name="authorizationCode">Authorization Code to authenticate.</param>
        /// <returns>Authentication Tokens along with User Data.</returns>
        [HttpGet]
        public async Task<IActionResult> Get(string authorizationCode)
        {
            var validationResult = await authorizationCodeProvider
                .ValidateCodeAsync(authorizationCode);

            if (!validationResult.IsSuccess)
            {
                return this.BadRequest(validationResult.ErrorMessage);
            }

            try
            {
                var userId = validationResult.Data.CodeValues.FirstOrDefault();

                var userData = await this.userManager.GetUserByIdAsync<TokenUserData>(new Guid(userId));

                if (!userData.IsSuccess)
                {
                    return this.BadRequest(userData.ErrorMessage);
                }

                var userRoles = await this.userManager.GetUserRoles(new Guid(userId));

                if (!userRoles.IsSuccess)
                {
                    return this.BadRequest(userRoles.ErrorMessage);
                }

                userData.Data.Roles = userRoles.Data;

                var isUserAdmin = userRoles.Data.Any(x => x.Equals(IdentityRoles.AdminRole));

                var result = await this.tokenProvider.GenerateTokenAsync(
                    validationResult.Data.ClientId,
                    new Guid(userId),
                    isUserAdmin);

                if (!result.IsSuccess)
                {
                    return this.BadRequest(result.ErrorMessage);
                }

                if (!isUserAdmin)
                {
                    var client = await this.userManager.GetUserSelectedClient(new Guid(userData.Data.UserId));

                    if (!client.IsSuccess)
                    {
                        return this.BadRequest(client);
                    }

                    userData.Data.ClientName = client.Data.Name;
                    userData.Data.ClientId = client.Data.Id.ToString();
                }

                await this.authorizationCodeProvider.MarkCodeAsUsed(authorizationCode);

                return this.Ok(new TokenGetResponseModel(
                    result.Data.AccessToken,
                    result.Data.RefreshToken,
                    result.Data.AccessTokenExpirationDate,
                    userData.Data));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Generates Authentication Tokens for a server-side authentication flow.
        /// </summary>
        /// <param name="model">Token Request Model.</param>
        /// <returns>Authentication Tokens.</returns>
        [HttpPost]
        public async Task<IActionResult> Get(TokenGetRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            if (!model.GrantType.Equals(IdentityAuthorizationGrantTypes.ClientCredentials))
            {
                return this.BadRequest("Invalid grant type.");
            }

            var validationResult = this.identityAuthorizationConfigProvider.ValidateClientCredentials(
                model.ClientId, model.ClientSecret);            

            try
            {
                var result = await this.tokenProvider.GenerateTokenAsync(
                    model.ClientId);

                if (!result.IsSuccess)
                {
                    return this.BadRequest(result.ErrorMessage);
                }

                return this.Ok(new TokenGetResponseModel(
                    result.Data.AccessToken,
                    result.Data.RefreshToken,
                    result.Data.AccessTokenExpirationDate));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Generates Authentication Tokens based on a provided refresh token.
        /// </summary>
        /// <param name="model">Refresh Token Request Model.</param>
        /// <returns>Authentication Tokens along with user data if available.</returns>
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshAccessToken(TokenRefreshRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            if (!model.GrantType.Equals(IdentityAuthorizationGrantTypes.RefreshToken))
            {
                return this.BadRequest("Invalid grant type.");
            }

            try
            {
                var validationResult = await this.tokenProvider.ValidateTokenAsync(
                    model.RefreshToken, true);

                if (!validationResult.IsSuccess)
                {
                    return this.BadRequest(validationResult.ErrorMessage);
                }

                var userRoles = await this.userManager.GetUserRoles(new Guid(validationResult.Data.UserId));

                var isUserAdmin = false;

                if (userRoles.IsSuccess)
                {
                    isUserAdmin = userRoles.Data.Any(x => x.Equals(IdentityRoles.AdminRole));
                }

                var result = await this.tokenProvider.RefreshAccessTokenAsync(
                    model.RefreshToken,
                    validationResult.Data.ClientId,
                    new Guid(validationResult.Data.UserId),
                    isUserAdmin);

                if (!result.IsSuccess)
                {
                    return this.BadRequest(result.ErrorMessage);
                }

                return this.Ok(new TokenRefreshResponseModel(
                    result.Data.AccessToken,
                    result.Data.RefreshToken,
                    result.Data.AccessTokenExpirationDate,
                    result.Data.RefreshTokenExpirationDate));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

#if DEBUG
        /// <summary>
        /// Generates Authentication Tokens for debugging purposes from a given user's email.
        /// </summary>
        /// <param name="email">Email of User to Authenticate.</param>
        /// <returns>Authentication Tokens.</returns>
        [HttpGet("{email}")]
        public async Task<IActionResult> GetDebugToken(string email)
        {
            try
            {
                var user = await this.userManager.GetUserByEmailAsync<TokenUserData>(email);
                if (user == null || !user.IsSuccess)
                {
                    return this.BadRequest();
                }

                var userRoles = await this.userManager.GetUserRoles(new Guid(user.Data.UserId));

                if (!userRoles.IsSuccess)
                {
                    return this.BadRequest(userRoles);
                }

                user.Data.Roles = userRoles.Data;

                var isUserAdmin = userRoles.Data.Any(x => x.Equals(IdentityRoles.AdminRole));

                var result = await this.tokenProvider.GenerateTokenAsync(
                    "mockclientid",
                    new Guid(user.Data.UserId),
                    isUserAdmin);

                if (!result.IsSuccess)
                {
                    return this.BadRequest(result);
                }

                if (!isUserAdmin)
                {
                    var client = await this.userManager.GetUserSelectedClient(new Guid(user.Data.UserId));

                    if (!client.IsSuccess)
                    {
                        return this.BadRequest(client);
                    }

                    user.Data.ClientName = client.Data.Name;
                    user.Data.ClientId = client.Data.Id.ToString();
                }

                return this.Ok(new TokenGetResponseModel(
                    result.Data.AccessToken,
                    result.Data.RefreshToken,
                    result.Data.AccessTokenExpirationDate,
                    user.Data));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
#endif
    }
}