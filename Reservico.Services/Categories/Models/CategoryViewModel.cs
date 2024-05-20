using AutoMapper;
using Reservico.Data.Entities;
using Reservico.Mapping;

namespace Reservico.Services.Categories.Models
{
    public class CategoryViewModel : IHaveCustomMapping
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, CategoryViewModel>()
                .ForMember(x => x.CategoryId, cfg => cfg.MapFrom(y => y.Id))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(y => y.Name));
        }
    }
}