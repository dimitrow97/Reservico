using AutoMapper;
using Reservico.Data.Entities;
using Reservico.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserManager.Models
{
    public class UserDetailsViewModel : IHaveCustomMapping
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Salutation { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public bool IsUsingDefaultPassword { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<UserClientViewModel> Clients { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<User, UserDetailsViewModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(y => y.Id))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(y => y.Email))
                .ForMember(x => x.FirstName, cfg => cfg.MapFrom(y => y.FirstName))
                .ForMember(x => x.LastName, cfg => cfg.MapFrom(y => y.LastName))
                .ForMember(x => x.PhoneNumber, cfg => cfg.MapFrom(y => y.PhoneNumber))
                .ForMember(x => x.IsActive, cfg => cfg.MapFrom(y => y.IsActive))
                .ForMember(x => x.IsUsingDefaultPassword, cfg => cfg.MapFrom(y => y.IsUsingDefaultPassword))
                .ForMember(x => x.Roles, cfg => cfg.MapFrom(y => y.UserRoles.Select(x => x.Role.Name)))
                .ForMember(x => x.Clients, cfg => cfg.MapFrom(y => y.UserClients.Select(uc => new UserClientViewModel { ClientId = uc.ClientId, ClientName = uc.Client.Name })));
        }
    }

    public class UserClientViewModel
    {
        public string ClientName { get; set; }

        public Guid ClientId { get; set; }
    }
}
