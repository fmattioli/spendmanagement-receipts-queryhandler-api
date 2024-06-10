using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Receipts.QueryHandler.CrossCutting.Config;

namespace Receipts.QueryHandler.Api.Extensions
{
    public static class KeycloakExtension
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration, KeycloakSettings keycloakSettings)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddKeycloakWebApi(configuration, "Settings:Keycloak");

            services
                .AddAuthorization()
                .AddKeycloakAuthorization()
                .AddAuthorizationBuilder()
                .AddPolicy(
                    "ApiReader",
                    policy =>
                    {
                        policy.RequireResourceRolesForClient(
                            keycloakSettings!.Resource!,
                            keycloakSettings!.Roles!.ToArray());
                    }
                );

            return services;
        }
    }
}
