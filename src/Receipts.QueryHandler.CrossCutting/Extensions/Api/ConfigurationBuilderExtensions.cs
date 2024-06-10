using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Receipts.QueryHandler.CrossCutting.Config;

namespace Receipts.QueryHandler.Api.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static Settings GetApplicationSettings(this IConfiguration configuration, IHostEnvironment env)
        {
            var settings = configuration.GetSection("Settings").Get<Settings>();

            if (!env.IsDevelopment())
            {
                settings!.MongoSettings!.ConnectionString = GetEnvironmentVariable("ConnectionString_Mongo");
            }

            return settings!;
        }

        private static string GetEnvironmentVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? "";
        }
    }
}
