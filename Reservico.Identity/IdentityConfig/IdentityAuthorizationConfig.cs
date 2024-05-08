namespace Reservico.Identity.IdentityConfig
{
    public class IdentityAuthorizationConfig
    {
        public string TokenIssuer { get; set; }

        public string TokenSalt { get; set; }

        public int AccessTokenExpirationMinutes { get; set; }

        public int RefreshTokenExpirationMinutes { get; set; }

        public int AuthorizationCodeExpirationMinutes { get; set; }

        public IEnumerable<IdentityCodeClientConfigElement> CodeClientConfigs { get; set; }

        public IdentityClientCredentialElement ClientCredentials { get; set; }
    }

    public class IdentityClientCredentialElement
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Name { get; set; }
    }

    public class IdentityCodeClientConfigElement
    {
        public string ClientId { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> AllowedRoles { get; set; }
    }
}