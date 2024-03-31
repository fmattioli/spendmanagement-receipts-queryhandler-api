using Receipts.ReadModel.CrossCutting;

namespace Receipts.ReadModel.API.Extensions
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
            }

            return settings!;
        }

        private static string GetEnvironmentVariableFromRender(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? "";
        }
    }
}
