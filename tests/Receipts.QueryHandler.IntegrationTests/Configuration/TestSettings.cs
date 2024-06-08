using Microsoft.Extensions.Configuration;

namespace Receipts.QueryHandler.IntegrationTests.Configuration
{
    public static class TestSettings
    {
        static TestSettings()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("Configuration/testsettings.json", false, true)
               .Build();

            Keycloak = config.GetSection("Keycloak").Get<KeycloakSettings>();
            MongoSettings = config.GetSection("MongoSettings").Get<MongoSettings>();
        }

        public static MongoSettings? MongoSettings { get; }
        public static KeycloakSettings? Keycloak { get; }
    }
}
