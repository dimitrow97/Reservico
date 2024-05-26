using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservico.Services.Reservations.Models
{
    public class ReservationViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Note { get; set; }
        public DateTime GuestsArrivingAt { get; set; }
        public int NumberOfGuests { get; set; }        
        public bool IsDeleted { get; set; }
        public Guid TableId { get; set; }
        public string TableName { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirmed { get; set; }
        public string Email { get; set; }
    }
}
