using Orionexx.Core.Shared.Primitives;

namespace Orionexx.Identity.Core.Entities.Account;

public class AppUser : BaseEntity<Guid>
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? UserName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? PasswordHash { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? SecurityStamp { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}