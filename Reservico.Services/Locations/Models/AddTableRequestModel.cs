using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Locations.Models
{
    public class AddTableRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Capacity { get; set; }
        public string Description { get; set; }

        [Required]
        public int WorkingHoursFrom { get; set; }

        [Required]
        public int WorkingHoursTo { get; set; }

        [Required]
        public bool CanTableTurn { get; set; }

        public int TableTurnOffset { get; set; }

        [Required]
        public Guid LocationId { get; set; }
    }
}
