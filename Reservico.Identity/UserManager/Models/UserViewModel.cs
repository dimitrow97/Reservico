using AutoMapper;
using Reservico.Data.Entities;
using Reservico.Mapping;

namespace Reservico.Identity.UserManager.Models
{
    public class UserViewModel : IHaveCustomMapping
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public bool IsUsingDefaultPassword { get; set; }

        public string ClientId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<User, UserViewModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(y => y.Id))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(y => y.Email))
                .ForMember(x => x.FullName, cfg => cfg.MapFrom(y => $"{y.FirstName} {y.LastName}"))
                .ForMember(x => x.IsActive, cfg => cfg.MapFrom(y => y.IsActive))
                .ForMember(x => x.IsUsingDefaultPassword, cfg => cfg.MapFrom(y => y.IsUsingDefaultPassword));
        }
    }
}