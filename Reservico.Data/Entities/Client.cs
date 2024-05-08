using Reservico.Data.Entities.Abstraction;

namespace Reservico.Data.Entities
{
    public class Client : BaseEntity
    {
        public Client()
        {
            UserClients = new HashSet<UserClient>();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }

        public virtual ICollection<UserClient> UserClients { get; set; }
    }
}
