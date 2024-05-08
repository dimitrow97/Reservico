using Reservico.Data.Entities.Abstraction;

namespace Reservico.Data.Entities
{
    public class UserClient : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public bool IsSelected { get; set; }

        public virtual User User { get; set; }
        public virtual Client Client { get; set; }
    }
}