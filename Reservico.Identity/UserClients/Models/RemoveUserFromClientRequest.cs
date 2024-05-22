using System;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserClients.Models
{
    public class RemoveUserFromClientRequest
    {
        public RemoveUserFromClientRequest(
            Guid userId,
            Guid clientId)
        {
            UserId = userId;
            ClientId = clientId;
        }

        public Guid UserId { get; set; }

        public Guid ClientId { get; set; }
    }
}
