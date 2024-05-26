using System;

namespace Reservico.Identity.UserClients.Models
{
    public class UserClientsViewModel
    {
        public Guid UserId { get; set; }

        public string ClientName { get; set; }

        public Guid ClientId { get; set; }

        public bool IsSelected { get; set; }

        public bool IsDeleted { get; set; }
    }
}