using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class LocationImage
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public string BlobPath { get; set; }
        public string BlobName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Location Location { get; set; }
    }
}
