using Reservico.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Reservico.Services.Clients.Models;
using Reservico.Common.Models;
using Reservico.Services.Categories;
using Reservico.Services.Categories.Models;

namespace Reservico.Api.Controllers.Administration
{
    public class CategoryAdministrationController : BaseAdministrationController
    {
        private readonly ICategoryService categoryService;
        private readonly ILogger<CategoryAdministrationController> logger;

        public CategoryAdministrationController(
            ICategoryService categoryService,
            ILogger<CategoryAdministrationController> logger)
        {
            this.categoryService = categoryService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates new Category.
        /// </summary>
        /// <param name="model">Category model to Create.</param>
        /// <returns>Status code indicating Success or Failure.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateCategoryRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            try
            {
                var response = await categoryService.Create(model);

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
        /// Deletes existing Category.
        /// </summary>
        /// <param name="categoryId">Id of the Category to delete.</param>
        /// <returns>Status code indicating Success or Failure.</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteClient(Guid categoryId)
        {
            try
            {
                var response = await this.categoryService.Delete(categoryId);

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
        /// Returns a list of all Categories.
        /// </summary>
        /// <returns>List of all Categories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<CategoryViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await this.categoryService.GetAll();

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
        /// Returns detailed model of specific Category.
        /// </summary>
        /// <param name="categoryId">Id of Category to return.</param>
        /// <returns>Detailed model of specific Category.</returns>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(ServiceResponse<CategoryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClient(Guid categoryId)
        {
            try
            {
                var response = await this.categoryService.Get(categoryId);

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