using CrossCutting.Config;

namespace API.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static Settings GetApplicationSettings(this IConfiguration configuration, IHostEnvironment env)
        {
            var settings = configuration.GetSection("Settings").Get<Settings>();

            if (!env.IsDevelopment())
            {
                settings!.MongoSettings!.ConnectionString = GetConnectionStringFromRenderSecret();
            }

            return settings!;
        }

        private static string GetConnectionStringFromRenderSecret()
        {
            return Environment.GetEnvironmentVariable("ConnectionString") ?? "";
        }
    }
}
