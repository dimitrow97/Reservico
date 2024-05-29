namespace Reservico.Services.LocationImages.Models
{
    public class LocationImagesViewModel
    {
        public Guid LocationId { get; set; }

        public IEnumerable<LocationImageViewModel> LocationImages { get; set; }
    }

    public class LocationImageViewModel
    {
        public Guid LocationImageId { get; set;}

        public string BlobPath { get; set;}
    }
}