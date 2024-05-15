using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class IdentityAuthorizationCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string ClientId { get; set; }
    }
}
