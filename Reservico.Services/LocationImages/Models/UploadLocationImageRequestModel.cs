using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.LocationImages.Models
{
    public class UploadLocationImageRequestModel
    {
        [Required]
        public Guid LocationId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
