using System;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class TokenRefreshResponseModel
    {
        public TokenRefreshResponseModel(
            string accessToken,
            string refreshToken, 
            DateTime accessTokenExpirationDate,
            DateTime refreshTokenExpirationDate)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.AccessTokenExpirationDate = accessTokenExpirationDate;
            this.RefreshTokenExpirationDate = refreshTokenExpirationDate;
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpirationDate { get; set; }

        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
