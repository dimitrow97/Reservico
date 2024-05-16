using Microsoft.AspNetCore.Mvc;
using Reservico.Identity.UserClients;
using Reservico.Common.Models;
using Reservico.Identity.UserClients.Models;

namespace Reservico.Api.Controllers.Client
{
    public class UserClientController : BaseClientController
    {
        private readonly IUserClientManager userClientManager;
        private readonly ILogger<UserClientController> logger;

        public UserClientController(
            IUserClientManager userClientManager,
            ILogger<UserClientController> logger)
        {
            this.userClientManager = userClientManager;
            this.logger = logger;
        }

        /// <summary>
        /// Gets User's clients for the Client dropdown selection.
        /// </summary>
        /// <returns>User's clients for the Client dropdown selection.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<UserClientsViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {           
            try
            {
                var userId = this.GetUserId();

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

        /// <summary>
        /// Marks User`s client as selected.
        /// </summary>
        /// <param name="model">Model to Mark User`s client as selected.</param>
        /// <returns>Status code indicating Success or Failure.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkClientAsSelected(MarkClientAsSelectedRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var userId = this.GetUserId();

                var response = await this.userClientManager
                    .MarkClientAsSelected(model, userId);

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
