using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Receipts.QueryHandler.Application.Providers
{
    public class AuthProvider(IHttpContextAccessor httpContextAccessor, JwtSecurityTokenHandler jwtSecurityTokenHandler) : IAuthProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly JwtSecurityTokenHandler _tokenHandler = jwtSecurityTokenHandler;

        public int GetTenant()
        {
            string jwtToken = GetToken();
            var tokenInfos = _tokenHandler.ReadJwtToken(jwtToken);
            var tenantClaim = tokenInfos.Claims.FirstOrDefault(c => c.Type == "tenant")?.Value!;
            return int.Parse(tenantClaim);
        }

        private string GetToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault();
            return authorizationHeader!.Replace("Bearer ", string.Empty);
        }

    }
}
