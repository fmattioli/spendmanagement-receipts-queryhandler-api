using Flurl;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Receipts.QueryHandler.Application.Constants;
using Receipts.QueryHandler.IntegrationTests.Configuration;
using System.Net;
using System.Text;

namespace Receipts.QueryHandler.IntegrationTests.Fixtures
{
    public class HttpFixture
    {
        private readonly HttpClient _httpClient;
        private string? accessToken;

        public HttpFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        public async Task<(HttpStatusCode StatusCode, string Content)> GetAsync<T>(string resource, T queryFilters, params string[] discardProperties) where T : class
        {
            await GenerateJWTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var queryParams = BuildFilters(queryFilters, discardProperties);

            var url = QueryHandlerConstants.ApiVersion
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

        private async Task<string> GenerateJWTokenAsync()
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                string tokenEndpoint = TestSettings.AuthSettings!.AuthServerUrl!;

                var parameters = new List<KeyValuePair<string, string>>
                {
                     new("grant_type", "password"),
                     new("client_id", TestSettings.AuthSettings!.Resource!),
                     new("client_secret", Environment.GetEnvironmentVariable("CLIENT_SECRET")!),
                     new("username", Environment.GetEnvironmentVariable("INTEGRATION_TESTS_USER")!),
                     new("password", Environment.GetEnvironmentVariable("INTEGRATION_TESTS_USER_PASSWORD")!),
                     new("scope", TestSettings.AuthSettings!.Scopes!.FirstOrDefault()!)
                };

                using var client = new HttpClient();
                var content = new FormUrlEncodedContent(parameters);
                using var response = await client.PostAsync(tokenEndpoint, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject tokenResponse = JObject.Parse(responseBody);
                accessToken = tokenResponse["access_token"]!.ToString();
                return accessToken!;
            }

            return accessToken;
        }
    }
}