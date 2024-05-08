using Reservico.Data.Entities.Abstraction;

namespace Reservico.Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            this.UserRoles = new HashSet<UserRole>();
            this.UserClients = new HashSet<UserClient>();
        }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool? IsUsingDefaultPassword { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserClient> UserClients { get; set; }
    }
}