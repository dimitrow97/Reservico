using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Locations.Models
{
    public class CreateLocationRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Email { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        public IEnumerable<Guid> Categories { get; set; }
    }
}
