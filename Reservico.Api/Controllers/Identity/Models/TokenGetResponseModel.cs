using Reservico.Identity.UserManager.Models;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class TokenGetResponseModel
    {
        public TokenGetResponseModel(
            string accessToken,
            string refreshToken,
            DateTime expirationDate)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.ExpirationDate = expirationDate;
        }

        public TokenGetResponseModel(
            string accessToken, 
            string refreshToken,
            DateTime expirationDate,
            TokenUserData userData)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.ExpirationDate = expirationDate;
            this.UserData = userData;
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpirationDate { get; set; }

        public TokenUserData UserData { get; set; }
    }
}
