using Flurl;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Receipts.QueryHandler.Application.Constants;
using Receipts.QueryHandler.IntegrationTests.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Receipts.QueryHandler.IntegrationTests.Fixtures
{
    public class HttpFixture
    {
        private readonly HttpClient _httpClient;

        public HttpFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        public async Task<(HttpStatusCode StatusCode, string Content)> GetAsync<T>(string resource, T queryFilters, params string[] discardProperties) where T : class
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJWToken());
            var queryParams = BuildFilters(queryFilters, discardProperties);

            var url = ReadModelConstants.ApiVersion
                .AppendPathSegment(resource)
                .AppendQueryParam(queryParams);

            using var response = await _httpClient.GetAsync(url);

            return (response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        private static IEnumerable<string> BuildFilters<T>(T queryFilters, string[] discardProperties) where T : class
        {
            return queryFilters
                .GetType()
                .GetProperties()
                .Where(p => discardProperties.Contains(p.Name))
                .Select(
                    propertyValue =>
                    {
                        var builder = new StringBuilder();
                        var value = propertyValue.GetValue(queryFilters);
                        if (value is IEnumerable<Guid> enumerableGuid)
                        {
                            builder.AppendJoin("&", enumerableGuid.Select(item => $"{propertyValue.Name}=" + item.ToString()));
                        }

                        if (value is IEnumerable<string> enumerableString)
                        {
                            builder.AppendJoin("&", enumerableString.Select(item => $"{propertyValue.Name}=" + item));
                        }

                        if (value is DateTime dateTime)
                        {
                            builder
                            .Append(propertyValue.Name)
                            .Append('=')
                            .Append(dateTime.Year)
                            .Append('-')
                            .Append(dateTime.Month)
                            .Append('-')
                            .Append(dateTime.Day);
                        }

                        return builder.ToString();
                    });
        }

        private static string GenerateJWToken()
        {
            var settings = TestSettings.JwtOptionsSettings;
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings?.SecurityKey ?? throw new Exception("Invalid token security key")));

            var claims = GenerateClaims();

            var credenciais = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                settings.Issuer,
                settings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(settings.AccessTokenExpiration),
                signingCredentials: credenciais
            );

            var tokenJWT = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenJWT;
        }

        private static List<Claim> GenerateClaims()
        {
            return
            [
                new(Application.Claims.ClaimTypes.Receipt, "Read"),
                new(Application.Claims.ClaimTypes.Category, "Read"),
            ];
        }
    }
}

