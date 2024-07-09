using Feijuca.Keycloak.MultiTenancy.Services.Models;
using Microsoft.Extensions.Configuration;
using Receipts.QueryHandler.CrossCutting.Config;

namespace Receipts.QueryHandler.IntegrationTests.Configuration
{
    public static class TestSettings
    {
        static TestSettings()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("Configuration/testsettings.json", false, true)
               .Build();

            AuthSettings = config.GetSection("AuthSettings").Get<AuthSettings>();
            MongoSettings = config.GetSection("MongoSettings").Get<MongoSettings>();
        }

        public static MongoSettings? MongoSettings { get; }
        public static AuthSettings? AuthSettings { get; }
    }
}
