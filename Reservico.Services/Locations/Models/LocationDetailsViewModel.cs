using Reservico.Services.LocationImages.Models;

namespace Reservico.Services.Locations.Models
{
    public class LocationDetailsViewModel : LocationViewModel
    {
        public int WorkingHoursFrom { get; set; }
        public int WorkingHoursTo { get; set; }

        public IEnumerable<LocationImageViewModel> LocationImages { get; set; }
    }
}