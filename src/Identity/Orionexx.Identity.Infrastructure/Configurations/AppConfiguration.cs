using System.ComponentModel.DataAnnotations;
using Orionexx.Identity.Application.Infrastructure.Configurations;

namespace Orionexx.Identity.Infrastructure.Configurations;

/// <summary>
/// App Configuration class accessible in infrastructure and application layer
/// </summary>
public class AppConfiguration : IAppConfiguration
{
    [Required] public ConnectionStrings ConnectionStrings { get; set; } = null!;
    [Required] public JwtSettings JwtSettings { get; set; } = null!;
    IConnectionStrings IAppConfiguration.ConnectionStrings => ConnectionStrings;
    IJwtSettings IAppConfiguration.JwtSettings => JwtSettings;
}