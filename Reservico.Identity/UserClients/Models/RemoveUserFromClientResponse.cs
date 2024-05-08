namespace Reservico.Identity.UserClients.Models
{
    public class RemoveUserFromClientResponse
    {
        public RemoveUserFromClientResponse(
            int userClientsCount)
        {
            UserClientsCount = userClientsCount;
        }

        public int UserClientsCount { get; set; }
    }
}