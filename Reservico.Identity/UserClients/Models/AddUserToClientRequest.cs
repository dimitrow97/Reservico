using System;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserClients.Models
{
    public class AddUserToClientRequest
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public Guid ClientId { get; set; }
    }
}