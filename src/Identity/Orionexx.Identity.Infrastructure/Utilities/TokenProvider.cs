using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Orionexx.Identity.Application.Infrastructure.Configurations;
using Orionexx.Identity.Application.Infrastructure.Utilities;

namespace Orionexx.Identity.Infrastructure.Utilities
{
    public class TokenProvider(
        IAppConfiguration appConfiguration) : ITokenProvider
    {
        public string GenerateToken(string userId, string email, string role, string jti, bool isAccessToken = true)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.JwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, jti),
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email, email),
                new("type", isAccessToken ? "access_token" : "refresh_token"),
                new(ClaimTypes.Role, role),
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: appConfiguration.JwtSettings.Issuer,
                audience: appConfiguration.JwtSettings.Audience,
                claims: claims,
                expires: isAccessToken ? DateTime.UtcNow.AddMinutes(appConfiguration.JwtSettings.AccessTokenExpirationMinutes) : DateTime.UtcNow.AddDays(appConfiguration.JwtSettings.RefreshTokenExpirationDays),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = appConfiguration.JwtSettings.Issuer,
                ValidAudience = appConfiguration.JwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.JwtSettings.SecretKey))
            };

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (!(validatedToken is JwtSecurityToken jwt) || !jwt.Claims.Any(c => c.Type == "type" && c.Value == "refresh_token"))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string? GenerateToken(ClaimsPrincipal? principal, bool isAccessToken = true)
        {
            if (principal is null)
                return null;
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal?.FindFirst(ClaimTypes.Email)?.Value;
            var role = principal?.FindFirst(ClaimTypes.Role)?.Value;
            var jti = principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(jti))
                return null;
            return GenerateToken(userId, email, role, jti, isAccessToken);
        }
    }
}
