using Microsoft.AspNetCore.Mvc;
using Reservico.Identity.UserClients;
using Reservico.Common.Models;
using Reservico.Identity.UserClients.Models;

namespace Reservico.Api.Controllers.Administration
{
    public class UserClientAdministrationController : BaseAdministrationController
    {
        private readonly IUserClientManager userClientManager;
        private readonly ILogger<UserClientAdministrationController> logger;

        public UserClientAdministrationController(
            IUserClientManager userClientManager,
            ILogger<UserClientAdministrationController> logger)
        {
            this.userClientManager = userClientManager;
            this.logger = logger;
        }

        /// <summary>
        /// Add existing User to Client.
        /// </summary>
        /// <param name="model">User Client Model to Add User-Client relation.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserToClient(AddUserToClientRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var response = await this.userClientManager.AddUserToClient(model);

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

        [HttpPost("Reactivate")]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReactivateUserToClient(AddUserToClientRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var response = await this.userClientManager.ReactivateUserClient(model);

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
        /// Gets User's clients for the Client dropdown selection.
        /// </summary>
        /// <returns>User's clients for the Linked Client section.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<UserClientsViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                var response = await this.userClientManager.GetUserClients(userId);

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
    }
}