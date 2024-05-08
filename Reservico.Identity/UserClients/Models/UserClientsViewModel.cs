using System;

namespace Reservico.Identity.UserClients.Models
{
    public class UserClientsViewModel
    {
        public string ClientName { get; set; }

        public Guid ClientId { get; set; }

        public bool IsSelected { get; set; }
    }
}