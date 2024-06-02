using Microsoft.AspNetCore.Mvc;
using Reservico.Common.Models;
using Reservico.Services.Categories;
using Reservico.Services.Categories.Models;

namespace Reservico.Api.Controllers.Public
{
    public class CategoryController : BasePublicController
    {
        private readonly ICategoryService categoryService;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            this.categoryService = categoryService;
            this.logger = logger;
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
    }
}