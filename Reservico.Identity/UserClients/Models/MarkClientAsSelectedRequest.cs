using System;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserClients.Models
{
    public class MarkClientAsSelectedRequest
    {
        [Required]
        public Guid ClientId { get; set; }
    }
}