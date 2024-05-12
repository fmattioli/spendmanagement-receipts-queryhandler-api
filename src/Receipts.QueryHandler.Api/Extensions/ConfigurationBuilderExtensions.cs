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
                settings!.MongoSettings!.ConnectionString = GetEnvironmentVariableFromRender("ConnectionString_Mongo");
                settings.TokenAuth = GetEnvironmentVariableFromRender("Token_Authentication");
                settings.SpendManagementIdentity!.Url = GetEnvironmentVariableFromRender("SpendManagementIdentity_Url");
            }

            return settings!;
        }

        private static string GetEnvironmentVariableFromRender(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? "";
        }
    }
}
