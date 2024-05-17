using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class Reservation
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Note { get; set; }
        public DateTime GuestsArrivingAt { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Guid TableId { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirmed { get; set; }
        public string Email { get; set; }

        public virtual Table Table { get; set; }
    }
}
