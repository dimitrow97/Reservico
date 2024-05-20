using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class Category
    {
        public Category()
        {
            LocationCategories = new HashSet<LocationCategory>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<LocationCategory> LocationCategories { get; set; }
    }
}
