namespace Receipts.QueryHandler.IntegrationTests.Configuration
{
    public class KeycloakSettings
    {
        public string? Realm { get; set; }
        public string? AuthServerUrl { get; set; }
        public string? SslRequired { get; set; }
        public string? Resource { get; set; }
        public bool VerifyTokenAudience { get; set; }
        public Credentials? Credentials { get; set; }
        public bool UseResourceRoleMappings { get; set; }
        public int ConfidentialPort { get; set; }
        public PolicyEnforcer? PolicyEnforcer { get; set; }
        public string? SwaggerTokenUrl { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public IEnumerable<string>? Scopes { get; set; }
    }

    public class Credentials
    {
        public string? Secret { get; set; }
    }

    public class PolicyEnforcer
    {
        public object? Credentials { get; set; }
    }
}
