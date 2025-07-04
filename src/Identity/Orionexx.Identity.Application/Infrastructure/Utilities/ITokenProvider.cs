using System.Security.Claims;

namespace Orionexx.Identity.Application.Infrastructure.Utilities;

public interface ITokenProvider
{
    string GenerateToken(string userId, string email, string role, string jti, bool isAccessToken = true);
    ClaimsPrincipal? GetPrincipalFromJwtToken(string token);
    string? GenerateToken(ClaimsPrincipal? principal, bool isAccessToken = true);
}
