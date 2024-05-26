using Reservico.Services.Reservations.Models;

namespace Reservico.Services.Locations.Models
{
    public class LocationDetailsViewModel : LocationViewModel
    {
        public IEnumerable<ReservationViewModel> LastFiveReservations { get; set; }
    }
}