namespace Reservico.Identity.Token.Models
{
    public class ValidateTokenResponse
    {
        public ValidateTokenResponse(
            string clientId)
        {
            ClientId = clientId;
        }

        public ValidateTokenResponse(
            string userId, string clientId)
        {
            UserId = userId;
            ClientId = clientId;
        }

        public string UserId { get; set; }

        public string ClientId { get; set; }
    }
}