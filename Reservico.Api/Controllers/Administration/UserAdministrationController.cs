using Microsoft.AspNetCore.Mvc;
using Reservico.Services.Clients;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserClients;
using Reservico.Common.Models;
using Reservico.Identity.UserManager.Models;
using Reservico.Identity.UserClients.Models;

namespace Reservico.Api.Controllers.Administration
{
    public class UserAdministrationController : BaseAdministrationController
    {
        private readonly IUserManager userManager;
        private readonly IClientService clientService;
        private readonly IUserClientManager userClientManager;
        private readonly ILogger<UserAdministrationController> logger;

        public UserAdministrationController(
            IUserManager userManager,
            IClientService clientService,
            IUserClientManager userClientManager,
            ILogger<UserAdministrationController> logger)
        {
            this.userManager = userManager;
            this.clientService = clientService;
            this.userClientManager = userClientManager;
            this.logger = logger;
        }       

        /// <summary>
        /// Returns specific User's details.
        /// </summary>
        /// <param name="userId">User Id to search for.</param>
        /// <returns>Specific User's details.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ServiceResponse<UserDetailsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                var response = await userManager.GetUserByIdAsync<UserDetailsViewModel>(new Guid(userId));

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
        /// Returns a filterable and pageable List of End Client Users.
        /// </summary>
        /// <param name="filter">optional filter parameter.</param>
        /// <param name="skip">optional paging parameter. Defaults to 0.</param>
        /// <param name="take">optional paging parameter. Defaults to 10.</param>
        /// <returns>Filterable, sortable and pageable List of End Client Users.</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ServiceResponse<ListViewModel<UserDetailsViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(string filter = null, int skip = 0, int take = 10)
        {
            try
            {
                var response = await userManager.Get(filter, skip, take);

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
        /// Registers new User for a specific client.
        /// </summary>
        /// <param name="model">User Model to Create.</param>
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
                if (!model.ClientId.HasValue)
                {
                    return BadRequest("Invalid Client Provided.");
                }

                var clientExists = await clientService.ClientExists(model.ClientId.Value);
                if (!clientExists.IsSuccess)
                {
                    return BadRequest("Invalid Client Provided.");
                }

                var initiatorId = this.GetUserId();

                var response = await userManager.RegisterAsync(model);

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
        /// Updates specific User's details.
        /// </summary>
        /// <param name="model">User Model to Update.</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(UserDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var initiatorId = this.GetUserId();

                var response = await userManager.UpdateAsync(model);

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
        /// Deletes a specific User.
        /// </summary>
        /// <param name="request">User model to Delete</param>
        /// <returns>Service Response indicating Success or Failure.</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(RemoveUserFromClientRequest request)
        {
            try
            {
                var ucDelete = await this.userClientManager
                    .RemoveUserFromClient(request);

                if (!ucDelete.IsSuccess)
                {
                    return BadRequest(ucDelete);
                }                

                return Ok(ServiceResponse.Success());

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
