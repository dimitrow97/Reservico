using Reservico.Common.Models;
using Reservico.Services.Categories.Models;

namespace Reservico.Services.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResponse> Create(
            CreateCategoryRequestModel model);

        Task<ServiceResponse> CategoryExists(Guid categoryId);            

        Task<ServiceResponse<CategoryViewModel>> Get(Guid categoryId);

        Task<ServiceResponse<IEnumerable<CategoryViewModel>>> GetAll();

        Task<ServiceResponse> Delete(Guid categoryId);
    }
}