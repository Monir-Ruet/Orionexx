using System.ComponentModel.DataAnnotations;
using Orionexx.Identity.Application.Interfaces.Configurations;

namespace Orionexx.Identity.Infrastructure.Configurations;

/// <summary>
/// ConnectionString Configuration For The Application
/// </summary>
public class ConnectionStrings : IConnectionStrings
{
    [Required] public string DatabaseName { get; set; } = null!;
    [Required] public string Orionexx { get; set; } = null!;
}