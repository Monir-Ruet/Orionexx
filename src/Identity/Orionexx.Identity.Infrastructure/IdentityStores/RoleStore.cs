using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orionexx.Identity.Infrastructure.Persistence;

namespace Orionexx.Identity.Infrastructure.IdentityStores;

public class RoleStore(ApplicationDbContext context) :
    IQueryableRoleStore<IdentityRole>,
    IRoleClaimStore<IdentityRole>
{
    public IQueryable<IdentityRole> Roles => throw new NotImplementedException();

    /// <summary>
    /// Adds a claim to a role.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="claim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task AddClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        await context.RoleClaims.AddAsync(new IdentityRoleClaim<string>
        {
            RoleId = role.Id,
            ClaimType = claim.Type,
            ClaimValue = claim.Value
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Creates the specified <paramref name="role"/> in the role store.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the create operation.</returns>
    public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        context.Roles.Add(role);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    /// <summary>
    /// Deletes the specified <paramref name="role"/> from the role store.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the delete operation.</returns>
    public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        context.Roles.Remove(role);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finds a role by its identifier.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the found role or null if not found.</returns>
    public async Task<IdentityRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(roleId, nameof(roleId));
        return await context.Roles.FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
    }

    /// <summary>
    /// Finds a role by its normalized name.
    /// </summary>
    /// <param name="normalizedRoleName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the found role or null if not found.</returns>
    public async Task<IdentityRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(normalizedRoleName, nameof(normalizedRoleName));
        return await context.Roles.FirstOrDefaultAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken);
    }

    /// <summary>
    /// Gets the list of claims associated with the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of claims.</returns>
    public async Task<IList<Claim>> GetClaimsAsync(IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        var claims = await context.RoleClaims
            .Where(rc => rc.RoleId == role.Id)
            .Select(rc => new Claim(rc.ClaimType!, rc.ClaimValue!))
            .ToListAsync(cancellationToken);
        return claims;
    }

    /// <summary>
    /// Gets the normalized name of the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the normalized role name.</returns>
    public Task<string?> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult(role.NormalizedName);
    }

    /// <summary>
    /// Gets the identifier of the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role identifier.</returns>
    public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult(role.Id);
    }

    /// <summary>
    /// Gets the name of the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role name.</returns>
    public Task<string?> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult(role.Name);
    }

    /// <summary>
    /// Removes a claim from a role.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="claim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task RemoveClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        var roleClaims = await context.RoleClaims
            .Where(rc => rc.RoleId == role.Id && rc.ClaimType == claim.Type && rc.ClaimValue == claim.Value)
            .ToListAsync(cancellationToken);
        if (roleClaims.Count != 0)
        {
            context.RoleClaims.RemoveRange(roleClaims);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Sets the normalized name of the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="normalizedName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetNormalizedRoleNameAsync(IdentityRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the name of the specified <paramref name="role"/>.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="roleName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetRoleNameAsync(IdentityRole role, string? roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        role.Name = roleName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Updates the specified <paramref name="role"/> in the role store.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(role);
        context.Roles.Update(role);
        return Task.FromResult(IdentityResult.Success);
    }
}
