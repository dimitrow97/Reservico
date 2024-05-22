using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Reservico.Common.Models;
using Reservico.Data.Entities;
using Reservico.Data.Interfaces;
using Reservico.Identity.IdentityConfig;
using Reservico.Identity.Token.Models;
using Reservico.Identity.UserManager;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Reservico.Identity.Token
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IRepository<IdentityToken> repository;
        private readonly ILogger<TokenProvider> logger;
        private readonly IIdentityAuthorizationConfigProvider identityAuthorizationConfigProvider;
        private readonly IUserManager userManager;

        public TokenProvider(
            IRepository<IdentityToken> repository,
            ILogger<TokenProvider> logger,
            IIdentityAuthorizationConfigProvider identityAuthorizationConfigProvider,
            IUserManager userManager)
        {
            this.repository = repository;
            this.logger = logger;
            this.identityAuthorizationConfigProvider = identityAuthorizationConfigProvider;
            this.userManager = userManager;
        }

        public async Task<ServiceResponse<GenerateTokenResponseModel>> GenerateTokenAsync(
            string clientId,
            Guid? userId = null,
            bool isUserAdmin = false)
        {
            var utcNow = DateTime.UtcNow;

            var identityAuthorizationConfig = identityAuthorizationConfigProvider
               .GetConfiguration();

            var accessTokenExpirationMinutes = 
                identityAuthorizationConfig.AccessTokenExpirationMinutes;

            var refreshTokenExpirationMinutes = 
                identityAuthorizationConfig.RefreshTokenExpirationMinutes;

            var token = new IdentityToken
            {
                TokenType = (int)TokenType.UserIdHolder,
                CreatedOn = utcNow,
                AccessTokenExpirationDate = utcNow.AddMinutes(
                    accessTokenExpirationMinutes),
                RefreshTokenExpirationDate = utcNow.AddMinutes(
                    refreshTokenExpirationMinutes)
            };

            if (!userId.HasValue)
            {
                token.TokenType = (int)TokenType.Base;
            }

            var roles = await this.GetUserRoles(userId);           

            token.AccessToken = this.GenerateToken(
                userId?.ToString(),
                identityAuthorizationConfig.TokenSalt,
                clientId,
                identityAuthorizationConfig.TokenIssuer,
                token.AccessTokenExpirationDate,
                utcNow,
                roles);

            token.RefreshToken = this.GenerateToken(
                userId?.ToString(),
                identityAuthorizationConfig.TokenSalt,
                clientId,
                identityAuthorizationConfig.TokenIssuer,
                token.RefreshTokenExpirationDate,
                utcNow);

            await repository.AddAsync(token);

            return ServiceResponse<GenerateTokenResponseModel>.Success(new GenerateTokenResponseModel
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                AccessTokenExpirationDate = token.AccessTokenExpirationDate,
                RefreshTokenExpirationDate = token.RefreshTokenExpirationDate
            });
        }

        public async Task<ServiceResponse<ValidateTokenResponse>> ValidateTokenAsync(
            string token, bool validateRefreshToken = false)
        {
            var tokenEntity = await repository
                .FindByConditionAsync(x => validateRefreshToken ? x.RefreshToken.Equals(token) : x.AccessToken.Equals(token));

            if (tokenEntity is null)
            {
                return ServiceResponse<ValidateTokenResponse>.Error("Invalid token.");
            }

            if (validateRefreshToken &&
                tokenEntity.RefreshTokenExpirationDate < DateTime.UtcNow)
            {
                return ServiceResponse<ValidateTokenResponse>.Error("Token has expired.");
            }
            else if (!validateRefreshToken &&
                tokenEntity.AccessTokenExpirationDate < DateTime.UtcNow)
            {
                return ServiceResponse<ValidateTokenResponse>.Error("Token has expired.");
            }

            var identityAuthorizationConfig = identityAuthorizationConfigProvider
                .GetConfiguration();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenSaltBytes = Convert.FromBase64String(identityAuthorizationConfig.TokenSalt);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenSaltBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var clientId = jwtToken.Claims.First(x => x.Type == "sub").Value;

                var result = new ValidateTokenResponse(clientId);

                if (tokenEntity.TokenType == (int)TokenType.UserIdHolder)
                {
                    var userId = jwtToken.Claims.First(x => x.Type == "jti").Value;
                    result.UserId = userId;

                    return ServiceResponse<ValidateTokenResponse>.Success(result);
                }

                return ServiceResponse<ValidateTokenResponse>.Success(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ServiceResponse<ValidateTokenResponse>.Error("Invalid or expired Token.");
            }
        }

        public async Task<ServiceResponse<RefreshAccessTokenResponseModel>> RefreshAccessTokenAsync(
            string refreshToken,
            string clientId,
            Guid? userId = null,
            bool isUserAdmin = false)
        {
            var tokenEntity = await repository
                .FindByConditionAsync(x => x.RefreshToken.Equals(refreshToken));

            if (tokenEntity is null)
            {
                return ServiceResponse<RefreshAccessTokenResponseModel>.Error("Invalid token.");
            }

            var result = await GenerateTokenAsync(clientId, userId, isUserAdmin);

            if (!result.IsSuccess)
            {
                return ServiceResponse<RefreshAccessTokenResponseModel>.Error(result.ErrorMessage);
            }

            return ServiceResponse<RefreshAccessTokenResponseModel>.Success(
                new RefreshAccessTokenResponseModel
                {
                    AccessToken = result.Data.AccessToken,
                    RefreshToken = result.Data.RefreshToken,
                    AccessTokenExpirationDate = result.Data.AccessTokenExpirationDate,
                    RefreshTokenExpirationDate = result.Data.RefreshTokenExpirationDate
                });
        }

        private string GenerateToken(
            string jti,
            string tokenSalt,
            string clientId,
            string issuer,
            DateTime expirationDate,
            DateTime createdOn,
            IList<string> roles = null)
        {
            var tokenSaltBytes = Convert.FromBase64String(tokenSalt);

            var role = string.IsNullOrWhiteSpace(jti) ? IdentityRoles.ApplicationRole : IdentityRoles.UserRole;

            if (roles?.Any() != true)
            {
                roles = new List<string>();
            }
            roles.Add(role);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, clientId),
                new Claim(JwtRegisteredClaimNames.Jti, jti ?? Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expirationDate.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, createdOn.ToString())
            };

            foreach (var currentRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, currentRole));
            }

            var handler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken
            (
                issuer,
                null,
                claims,
                createdOn.AddMilliseconds(-30),
                expirationDate,
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenSaltBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            );

            return handler.WriteToken(token);
        }

        private async Task<IList<string>> GetUserRoles(Guid? userId)
        { 
            if (!userId.HasValue)
            {
                return null;
            }

            var result = await userManager.GetUserRoles(userId.Value);

            if (result.IsSuccess)
            {
                return result.Data.ToList();
            }

            return null;
        }
    }
}