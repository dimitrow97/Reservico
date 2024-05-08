using Reservico.Common.Models;
using Reservico.Identity.Token.Models;

namespace Reservico.Identity.Token
{
    public interface ITokenProvider
    {
        Task<ServiceResponse<GenerateTokenResponseModel>> GenerateTokenAsync(
            string clientId,
            Guid userId,
            bool isUserAdmin = false);

        Task<ServiceResponse<ValidateTokenResponse>> ValidateTokenAsync(
            string token,
            bool validateRefreshToken = false);

        Task<ServiceResponse<RefreshAccessTokenResponseModel>> RefreshAccessTokenAsync(
            string refreshToken,
            string clientId,
            Guid userId,
            bool isUserAdmin = false);
    }
}