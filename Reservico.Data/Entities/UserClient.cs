using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class UserClient
    {
        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public bool IsSelected { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Client Client { get; set; }
        public virtual User User { get; set; }
    }
}
