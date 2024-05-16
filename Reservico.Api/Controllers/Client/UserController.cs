using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Identity.UserClients;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserManager.Models;

namespace Reservico.Api.Controllers.Client
{
    public class UserController : BaseClientController
    {
        private readonly IUserManager userManager;
        private readonly IUserClientManager userClientManager;
        private readonly ILogger<UserController> logger;

        public UserController(
            IUserManager userManager,
            IUserClientManager userClientManager,
            ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.userClientManager = userClientManager;
            this.logger = logger;
        }

        /// <summary>
        /// Registers new User to the Current User`s Client.
        /// </summary>
        /// <param name="model">User Model to Create. ClientId property should be null.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser(UserRegistrationRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var userId = this.GetUserId();

                var userClients = await this.userClientManager.GetUserClients(userId);

                if (!userClients.IsSuccess)
                {
                    return BadRequest(userClients.ErrorMessage);
                }

                model.ClientId = userClients.Data.FirstOrDefault(x => x.IsSelected)?.ClientId;

                var response = await this.userManager.RegisterAsync(model);

                if (!response.IsSuccess)
                {
                    return this.BadRequest(response);
                }                              

                return this.Ok(response);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
