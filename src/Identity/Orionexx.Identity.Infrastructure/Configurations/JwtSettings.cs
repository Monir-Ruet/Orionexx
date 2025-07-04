using System.ComponentModel.DataAnnotations;
using Orionexx.Identity.Application.Infrastructure.Configurations;

namespace Orionexx.Identity.Infrastructure.Configurations;

/// <summary>
/// JsonWebToken Bearer Configuration Settings 
/// </summary>
public class JwtSettings : IJwtSettings
{
    [Required]
    public string SecretKey { get; init; } = null!;
    [Required]
    public string Issuer { get; init; } = null!;
    [Required]
    public string Audience { get; init; } = null!;
    public int AccessTokenExpirationMinutes { get; init; }
    public int RefreshTokenExpirationDays { get; init; }
}