using Contracts.Web.ServiceCollectionExtensions.KeycloakAuth;

namespace Receipts.QueryHandler.CrossCutting.Config
{
    public interface ISettings
    {
        public AuthSettings? AuthSettings { get; }
        public TracingSettings? TracingSettings { get; }
        public MongoSettings? MongoSettings { get; }
    }

    public record Settings : ISettings
    {
        public AuthSettings? AuthSettings { get; set; }
        public TracingSettings? TracingSettings { get; set; }
        public string TokenAuth { get; set; } = null!;
        public MongoSettings? MongoSettings { get; set; }
    }
}
