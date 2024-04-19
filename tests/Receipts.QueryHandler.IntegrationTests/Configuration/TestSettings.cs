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

            JwtOptionsSettings = config.GetSection("JwtOptionsSettings").Get<JwtOptionsSettings>();
            MongoSettings = config.GetSection("MongoSettings").Get<MongoSettings>();
        }

        public static MongoSettings? MongoSettings { get; }
        public static JwtOptionsSettings? JwtOptionsSettings { get; }
    }
}
