using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Services.Categories.Models;

namespace Reservico.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Category> categoryRepository;

        public CategoryService(
            IMapper mapper,
            IRepository<Category> categoryRepository)
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse> Create(
            CreateCategoryRequestModel model)
        {
            var categoryExists = await this.categoryRepository.Query()
                .Where(c => !c.IsDeleted).AnyAsync(x => x.Name.Equals(model.Name));

            if (categoryExists)
            {
                return ServiceResponse
                    .Error("Category with the same name already exists");
            }

            var category = new Category
            {
                Name = model.Name,
                CreatedOn = DateTime.UtcNow
            };

            await this.categoryRepository.AddAsync(category);

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse> CategoryExists(Guid categoryId)
        {
            var categoryExists = await this.categoryRepository.Query()
                .Where(c => !c.IsDeleted).AnyAsync(x => x.Id.Equals(categoryId));

            if (!categoryExists)
            {
                return ServiceResponse
                    .Error("Category does NOT exists");
            }

            return ServiceResponse.Success();
        }

        public async Task<ServiceResponse<CategoryViewModel>> Get(Guid categoryId)
        {
            var category = await this.categoryRepository.Query()
                .FirstOrDefaultAsync(x => x.Id.Equals(categoryId) && !x.IsDeleted);

            if (category is null)
            {
                return ServiceResponse<CategoryViewModel>.
                    Error("Category does NOT exists");
            }

            var result = this.mapper.Map(category, new CategoryViewModel());

            return ServiceResponse<CategoryViewModel>.Success(result);
        }

        public async Task<ServiceResponse<IEnumerable<CategoryViewModel>>> GetAll()
        {           
            var categories = await this.categoryRepository.Query()
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return ServiceResponse<IEnumerable<CategoryViewModel>>
                .Success(categories.Select(x => this.mapper.Map(x, new CategoryViewModel())));
        }

        public async Task<ServiceResponse> Delete(Guid categoryId)
        {
            var categoryExists = await this.CategoryExists(categoryId);

            if (!categoryExists.IsSuccess)
            {
                return ServiceResponse
                    .Error(categoryExists.ErrorMessage);
            }

            var category = await this.categoryRepository.GetByIdAsync(categoryId);

            category.IsDeleted = true;
            category.UpdatedOn = DateTime.UtcNow;

            await this.categoryRepository.SaveChangesAsync();

            return ServiceResponse.Success();
        }
    }
}