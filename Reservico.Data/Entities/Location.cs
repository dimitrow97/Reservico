using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class Location
    {
        public Location()
        {
            Tables = new HashSet<Table>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Guid ClientId { get; set; }
        public string Email { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Table> Tables { get; set; }
    }
}
