using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orionexx.Core.Shared.Entities.Events;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        modelBuilder.Entity<IdentityUserLogin<string>>()
        .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new { r.UserId, r.RoleId });

        modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<IdentityRole> Roles => Set<IdentityRole>();
    public DbSet<IdentityUserClaim<string>> UserClaims => Set<IdentityUserClaim<string>>();
    public DbSet<IdentityUserLogin<string>> UserLogins => Set<IdentityUserLogin<string>>();
    public DbSet<IdentityUserToken<string>> UserTokens => Set<IdentityUserToken<string>>();
    public DbSet<IdentityRoleClaim<string>> RoleClaims => Set<IdentityRoleClaim<string>>();
    public DbSet<IdentityUserRole<string>> UserRoles => Set<IdentityUserRole<string>>();
    public DbSet<Event> Events => Set<Event>();
}