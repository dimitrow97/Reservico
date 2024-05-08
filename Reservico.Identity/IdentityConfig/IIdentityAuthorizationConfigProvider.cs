using Reservico.Common.Models;

namespace Reservico.Identity.IdentityConfig
{
    public interface IIdentityAuthorizationConfigProvider
    {
        ServiceResponse ValidateClientCredentials(
            string clientId,
            string clientSecret);

        ServiceResponse ValidateClientConfig(string clientId);

        ServiceResponse ValidateUserAccess(string clientId, IEnumerable<string> userRoles);

        IdentityAuthorizationConfig GetConfiguration();
    }
}
