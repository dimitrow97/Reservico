using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
