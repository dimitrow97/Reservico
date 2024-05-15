using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class User
    {
        public User()
        {
            UserClients = new HashSet<UserClient>();
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsUsingDefaultPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserClient> UserClients { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
