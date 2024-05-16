using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Reservico.Identity.UserPasswordManager.Models;
using Reservico.Api.Controllers.Identity.Models;
using Reservico.Common.Models;
using Reservico.Identity.UserManager.Models;
using Reservico.Identity.UserPasswordManager;
using Reservico.Services.Clients;

namespace Reservico.Api.Controllers.Common
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "Common Endpoints")]
    public class UsersController : BaseCommonController
    {
        private readonly IClientService clientService;
        private readonly IUserPasswordManager userPasswordManager;
        private readonly ILogger<UsersController> logger;

        public UsersController(
            IClientService clientService,
            IUserPasswordManager userPasswordManager,
            ILogger<UsersController> logger)
        {
            this.clientService = clientService;
            this.userPasswordManager = userPasswordManager;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a list of Users registered to a specific Client.
        /// </summary>
        /// <param name="clientId">Id of Client to search for.</param>
        /// <returns>List of Users registered to a specific Client.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<UserViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid clientId)
        {
            try
            {
                var response = await clientService.GetClientUsers(clientId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Changes user's password.
        /// </summary>
        /// <param name="model">Change Password Request Model.</param>
        /// <returns>Status Result indicating Success or Failure.</returns>
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var userId = this.GetUserId();

                var response = await this.userPasswordManager.ChangePasswordAsync(model, new Guid(userId));

                if (!response.IsSuccess)
                {
                    return this.BadRequest(response);
                }

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Triggers Forgot Password flow for given email.
        /// </summary>
        /// <param name="model">Forgot password request model</param>
        /// <returns>Status Result indicating Success or Failure.</returns>
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var response = await this.userPasswordManager.ResetPasswordAsync(model.Email);

                if (!response.IsSuccess)
                {
                    return this.BadRequest(response);
                }

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Resets User's password for a Forgot Password flow.
        /// </summary>
        /// <param name="model">Reset Password Request Model.</param>
        /// <returns>Status Result indicating Success or Failure.</returns>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var response = await this.userPasswordManager.ResetPasswordAsync(model);

                if (!response.IsSuccess)
                {
                    return this.BadRequest(response);
                }

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}