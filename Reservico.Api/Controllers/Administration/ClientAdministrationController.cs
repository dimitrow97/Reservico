using Reservico.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Reservico.Services.Clients.Models;
using Reservico.Common.Models;

namespace Reservico.Api.Controllers.Administration
{
    public class ClientAdministrationController : BaseAdministrationController
    {
        private readonly IClientService clientService;
        private readonly ILogger<ClientAdministrationController> logger;

        public ClientAdministrationController(
            IClientService clientService,
            ILogger<ClientAdministrationController> logger)
        {
            this.clientService = clientService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates new Client.
        /// </summary>
        /// <param name="model">Client model to Create.</param>
        /// <returns>Service Response containing Client Details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse<ClientDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateClientRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var userId = this.GetUserId();

                var response = await clientService.CreateClient(model, userId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }
                var clientResponse = await clientService.GetClient(response.Data);
                if (!clientResponse.IsSuccess)
                {
                    return BadRequest(clientResponse);
                }

                return Ok(clientResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates existing Client.
        /// </summary>
        /// <param name="model">Client model to Update.</param>
        /// <returns>Status code indicating Success or Failure.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateClient(UpdateClientRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var userId = this.GetUserId();

                var response = await clientService.UpdateClient(model, userId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes existing Client.
        /// </summary>
        /// <param name="clientId">Id of the client to delete.</param>
        /// <returns>Status code indicating Success or Failure.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteClient(Guid clientId)
        {
            try
            {
                var userId = this.GetUserId();

                var response = await clientService.DeleteClient(clientId, userId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns a list of all Clients.
        /// </summary>
        /// <returns>List of all Clients.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<ClientViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var response = await clientService.GetAllClients();

                if (!response.IsSuccess)
                {
                    return BadRequest(response.ErrorMessage);
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
        /// Returns detailed model of specific Client.
        /// </summary>
        /// <param name="clientId">Id of Client to return.</param>
        /// <returns>Detailed model of specific Client.</returns>
        [HttpGet("{clientId}")]
        [ProducesResponseType(typeof(ServiceResponse<ClientDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClient(Guid clientId)
        {
            try
            {
                var response = await clientService.GetClient(clientId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response.ErrorMessage);
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
        /// Returns a Client's name.
        /// </summary>
        /// <param name="clientId">Id of Client to return.</param>
        /// <returns>Client's name.</returns>
        [HttpGet("GetName/{clientId}")]
        [ProducesResponseType(typeof(ServiceResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClientName(Guid clientId)
        {
            try
            {
                var response = await clientService.GetClientName(clientId);

                if (!response.IsSuccess)
                {
                    return BadRequest(response.ErrorMessage);
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