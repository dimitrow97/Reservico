using System;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserClients.Models
{
    public class AddUserToClientRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ClientId { get; set; }
    }
}