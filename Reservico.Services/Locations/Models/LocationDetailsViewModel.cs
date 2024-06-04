using Reservico.Services.LocationImages.Models;

namespace Reservico.Services.Locations.Models
{
    public class LocationDetailsViewModel : LocationViewModel
    {
        public int WorkingHoursFrom { get; set; } = 10;
        public int WorkingHoursTo { get; set; } = 23;

        public IEnumerable<LocationImageViewModel> LocationImages { get; set; }
    }
}