using Reservico.Data.Entities;

namespace Reservico.Services.Locations.Models
{
    public class LocationDetailsViewModel : LocationViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Table> Tables { get; set; }

        public IEnumerable<Reservation> LastFiveReservations { get; set; }
    }
}
