namespace Reservico.Identity.Token.Models
{
    public class GenerateTokenResponseModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpirationDate { get; set; }

        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}