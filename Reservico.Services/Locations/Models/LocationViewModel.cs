using AutoMapper;
using Reservico.Data.Entities;
using Reservico.Mapping;
using Reservico.Services.Categories.Models;

namespace Reservico.Services.Locations.Models
{
    public class LocationViewModel : IHaveCustomMapping
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string ClientName { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Location, LocationViewModel>()
                .ForMember(x => x.LocationId, cfg => cfg.MapFrom(y => y.Id))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(y => y.Name))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(y => y.Email))
                .ForMember(x => x.Address, cfg => cfg.MapFrom(y => y.Address))
                .ForMember(x => x.City, cfg => cfg.MapFrom(y => y.City))
                .ForMember(x => x.Postcode, cfg => cfg.MapFrom(y => y.Postcode))
                .ForMember(x => x.Country, cfg => cfg.MapFrom(y => y.Country))
                .ForMember(x => x.ClientName, cfg => cfg.MapFrom(y => y.Client.Name))
                .ForMember(x => x.Categories, cfg => cfg.MapFrom(y => y.LocationCategories.Select(z => new CategoryViewModel { CategoryId = z.Category.Id, Name = z.Category.Name })));
        }
    }
}