using Microsoft.Extensions.Options;
using Reservico.Common.Models;

namespace Reservico.Identity.IdentityConfig
{
    public class IdentityAuthorizationConfigProvider : IIdentityAuthorizationConfigProvider
    {
        private readonly IdentityAuthorizationConfig identityAuthorizationConfig;

        public IdentityAuthorizationConfigProvider(
            IOptions<IdentityAuthorizationConfig> identityAuthorizationConfig)
        {
            this.identityAuthorizationConfig = identityAuthorizationConfig.Value;
        }

        public ServiceResponse ValidateClientCredentials(
            string clientId,
            string clientSecret)
        {

            var config = identityAuthorizationConfig.ClientCredentials;
            if (!config.ClientId.Equals(clientId) ||
                !config.ClientSecret.Equals(clientSecret))
            {
                return ServiceResponse.Error($"Invalid {nameof(clientId)} and/or {nameof(clientSecret)}");
            }

            return ServiceResponse.Success();
        }

        public IdentityAuthorizationConfig GetConfiguration()
        {
            return identityAuthorizationConfig;
        }

        public ServiceResponse ValidateClientConfig(string clientId)
        {
            if (!identityAuthorizationConfig.CodeClientConfigs.Any(x => x.ClientId.Equals(clientId)))
            {
                return ServiceResponse.Error($"Invalid {nameof(clientId)}");
            }

            return ServiceResponse.Success();
        }

        public ServiceResponse ValidateUserAccess(
            string clientId, 
            IEnumerable<string> userRoles)
        {
            var clientConfig = identityAuthorizationConfig.CodeClientConfigs
                .FirstOrDefault(x => x.ClientId.Equals(clientId));

            if (clientConfig is null)
            {
                return ServiceResponse.Error($"Invalid {nameof(clientId)}");
            }

            if (userRoles is null || !userRoles.Any())
            {
                return ServiceResponse.Error($"User has no roles.");
            }

            var result = userRoles.Any(x => clientConfig.AllowedRoles.Any(ar => ar.Equals(x)));

            if (!result)
            {
                return ServiceResponse.Error($"User not allowed.");
            }

            return ServiceResponse.Success();
        }
    }
}
