using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Categories.Models
{
    public class CreateCategoryRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}