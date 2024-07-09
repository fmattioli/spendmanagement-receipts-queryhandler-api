﻿using Feijuca.Keycloak.MultiTenancy.Extensions;
using Feijuca.Keycloak.MultiTenancy.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Receipts.QueryHandler.CrossCutting.Extensions.Auth
{
    public static class AuthExtension
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, AuthSettings authSettings)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddKeyCloakAuth(authSettings!);

            return services;
        }
    }
}
