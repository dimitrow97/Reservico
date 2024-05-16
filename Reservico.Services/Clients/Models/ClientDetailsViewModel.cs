namespace Reservico.Services.Clients.Models
{
    public class ClientDetailsViewModel : ClientViewModel
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }
    }
}