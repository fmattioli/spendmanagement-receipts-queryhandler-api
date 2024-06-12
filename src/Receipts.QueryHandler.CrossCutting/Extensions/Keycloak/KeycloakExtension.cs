using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Receipts.QueryHandler.CrossCutting.Config;
using System.IdentityModel.Tokens.Jwt;

namespace Receipts.QueryHandler.Api.Extensions
{
    public static class KeycloakExtension
    {
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, KeycloakSettings keycloakSettings)
        {
            var httpClient = new HttpClient();
            var tokenHandler = new JwtSecurityTokenHandler();
            var realms = new[]
            {
                new { Realm = "10000", Issuer = "https://docker-containers-keycloak.8ya11r.easypanel.host/realms/10000", Audience = keycloakSettings.Resource, ClientId = keycloakSettings.Resource },
                new { Realm = "11000", Issuer = "https://docker-containers-keycloak.8ya11r.easypanel.host/realms/11000", Audience = keycloakSettings.Resource, ClientId = keycloakSettings.Resource }
            };

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddKeycloakWebApi(
                    options =>
                    {
                        options.Resource = keycloakSettings.Resource!;
                        options.AuthServerUrl = keycloakSettings.AuthServerUrl;
                        options.VerifyTokenAudience = true;
                    },
                    options =>
                    {
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = async context =>
                            {
                                try
                                {
                                    var bearerToken = context.Request.Headers.Authorization.FirstOrDefault()!.Replace("Bearer ", "");
                                    var tokenInfos = tokenHandler.ReadJwtToken(bearerToken);
                                    var tenantClaim = tokenInfos.Claims.FirstOrDefault(c => c.Type == "tenant")?.Value;

                                    var realmConfig = realms.FirstOrDefault(r => r.Realm == tenantClaim);
                                    if (realmConfig is null)
                                    {
                                        context.NoResult();
                                        return;
                                    }

                                    var jwksUrl = $"{realmConfig.Issuer}/protocol/openid-connect/certs";

                                    var jwks = await httpClient.GetStringAsync(jwksUrl);
                                    var jsonWebKeySet = new JsonWebKeySet(jwks);

                                    var tokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidateIssuer = true,
                                        ValidIssuer = realmConfig.Issuer,
                                        ValidateAudience = true,
                                        ValidAudience = realmConfig.Audience,
                                        ValidateLifetime = true,
                                        ValidateIssuerSigningKey = true,
                                        IssuerSigningKeys = jsonWebKeySet.Keys
                                    };

                                    var claims = tokenHandler.ValidateToken(bearerToken, tokenValidationParameters, out var validatedToken);
                                    context.Principal = claims;
                                    context.Success();
                                }
                                catch (Exception)
                                {
                                    context.Fail("Token validation failed");
                                }
                            }
                        };
                    }
                );

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
