using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Reservations.Models
{
    public class MakeReservationRequestModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        public string Note { get; set; }

        [Required]
        public DateTime GuestsArrivingAt { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        [Required]
        public Guid LocationId { get; set; }
    }
}
