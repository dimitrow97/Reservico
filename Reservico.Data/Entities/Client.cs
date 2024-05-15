using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class Client
    {
        public Client()
        {
            UserClients = new HashSet<UserClient>();
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

        public virtual ICollection<UserClient> UserClients { get; set; }
    }
}
