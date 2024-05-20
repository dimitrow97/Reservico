using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class LocationCategory
    {
        public Guid LocationId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category Category { get; set; }
        public virtual Location Location { get; set; }
    }
}
