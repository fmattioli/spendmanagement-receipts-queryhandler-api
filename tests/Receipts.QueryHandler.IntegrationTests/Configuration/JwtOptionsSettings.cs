namespace Receipts.QueryHandler.IntegrationTests.Configuration
{
    public class JwtOptionsSettings
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string SecurityKey { get; set; } = null!;
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
