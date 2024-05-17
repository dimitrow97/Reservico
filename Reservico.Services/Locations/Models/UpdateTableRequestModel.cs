using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Locations.Models
{
    public class UpdateTableRequestModel
    {
        public Guid TableId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public int WorkingHoursFrom { get; set; }
        public int WorkingHoursTo { get; set; }
        public bool CanTableTurn { get; set; }
        public int TableTurnOffset { get; set; }
    }
}
