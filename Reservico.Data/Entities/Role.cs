using Reservico.Data.Entities.Abstraction;

namespace Reservico.Data.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}