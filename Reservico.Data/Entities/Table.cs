using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class Table
    {
        public Table()
        {
            Reservations = new HashSet<Reservation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Guid LocationId { get; set; }
        public int WorkingHoursFrom { get; set; }
        public int WorkingHoursTo { get; set; }
        public bool CanTableTurn { get; set; }
        public int TableTurnOffset { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
