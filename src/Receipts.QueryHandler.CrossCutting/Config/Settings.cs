namespace Receipts.QueryHandler.CrossCutting.Config
{
    public interface ISettings
    {
        public KeycloakSettings? Keycloak { get; }
        public TracingSettings? TracingSettings { get; }
        public MongoSettings? MongoSettings { get; }
    }

    public record Settings : ISettings
    {
        public KeycloakSettings? Keycloak { get; set; }
        public TracingSettings? TracingSettings { get; set; }
        public string TokenAuth { get; set; } = null!;
        public MongoSettings? MongoSettings { get; set; }
    }
}
