using Microsoft.OpenApi.Models;
using Receipts.QueryHandler.CrossCutting.Config;
using Receipts.QueryHandler.CrossCutting.Filters;

namespace Receipts.QueryHandler.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services, KeycloakSettings keyCloakSettings)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Receipts.QueryHandler.Api", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(keyCloakSettings.SwaggerTokenUrl!),
                            Scopes = keyCloakSettings.Scopes!.ToDictionary(key => key, value => value)
                        }
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Receipts.QueryHandler.Api.xml"));
            });
        }
    }
}
