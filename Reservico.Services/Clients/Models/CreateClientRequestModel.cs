using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Clients.Models
{
    public class CreateClientRequestModel
    {
        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        [Required]
        public string Country { get; set; }
    }    
}