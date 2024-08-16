using Feijuca.Keycloak.MultiTenancy.Services.Models;

namespace Receipts.QueryHandler.CrossCutting.Config
{
    public interface ISettings
    {
        public AuthSettings AuthSettings { get; }
        public MongoSettings MongoSettings { get; }
        public MltConfigsSettings MltConfigsSettings { get; }
    }

    public record Settings : ISettings
    {
        public required AuthSettings AuthSettings { get; set; }
        public required string TokenAuth { get; set; }
        public required MongoSettings MongoSettings { get; set; }
        public required MltConfigsSettings MltConfigsSettings { get; set; }
    }
}
