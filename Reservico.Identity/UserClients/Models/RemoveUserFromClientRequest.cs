using System;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserClients.Models
{
    public class RemoveUserFromClientRequest
    {
        public RemoveUserFromClientRequest(
            string userId,
            Guid clientId)
        {
            UserId = userId;
            ClientId = clientId;
        }

        public string UserId { get; set; }

        public Guid ClientId { get; set; }
    }
}
