using AutoMapper;
using Reservico.Mapping;
using Reservico.Data.Entities;

namespace Reservico.Identity.UserManager.Models
{
    public class TokenUserData : IHaveCustomMapping
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ClientName { get; set; }

        public string ClientId { get; set; }

        public bool IsUsingDefaultPassword { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<User, TokenUserData>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(y => y.Id))
                .ForMember(x => x.FirstName, cfg => cfg.MapFrom(y => y.FirstName))
                .ForMember(x => x.LastName, cfg => cfg.MapFrom(y => y.LastName))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(y => y.Email))
                .ForMember(x => x.IsUsingDefaultPassword, cfg => cfg.MapFrom(y => y.IsUsingDefaultPassword));
        }
    }
}