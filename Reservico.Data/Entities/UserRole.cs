using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
