using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Infrastructure.Persistence;

namespace Orionexx.Identity.Infrastructure.IdentityStores;

public class UserStore(ApplicationDbContext context) :
    IQueryableUserStore<AppUser>,
    IUserEmailStore<AppUser>,
    IUserLoginStore<AppUser>,
    IUserPasswordStore<AppUser>,
    IUserPhoneNumberStore<AppUser>,
    IUserSecurityStampStore<AppUser>,
    IUserClaimStore<AppUser>,
    IUserLockoutStore<AppUser>,
    IUserRoleStore<AppUser>,
    IUserAuthenticationTokenStore<AppUser>
{
    public IQueryable<AppUser> Users => context.Users;

    /// <summary>
    /// Adds the specified <paramref name="claims"/> to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="claims"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task AddClaimsAsync(AppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(claims, nameof(claims));

        var userClaimsList = await context.UserClaims
            .Where(uc => uc.UserId.ToString() == user.Id.ToString())
            .ToListAsync(cancellationToken);
        var userClaims = userClaimsList.ToDictionary(uc => (uc.ClaimType, uc.ClaimValue));
        var newClaims = new List<IdentityUserClaim<string>>();
        if (userClaims.Count != 0)
        {
            claims = claims.Where(c => !userClaims.ContainsKey((c.Type, c.Value)));
            newClaims.AddRange(claims.Select(c => new IdentityUserClaim<string>
            {
                UserId = user.Id.ToString(),
                ClaimType = c.Type,
                ClaimValue = c.Value
            }));
            context.UserClaims.AddRange(newClaims);
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a <see cref="UserLoginInfo"/> to a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="login"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task AddLoginAsync(AppUser user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(login, nameof(login));
        var userLogin = new IdentityUserLogin<string>
        {
            UserId = user.Id.ToString(),
            ProviderKey = login.ProviderKey,
            LoginProvider = login.LoginProvider,
            ProviderDisplayName = login.ProviderDisplayName
        };
        context.UserLogins.Add(userLogin);
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sets the email address for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetEmailAsync(AppUser user, string? email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        user.Email = email;
        user.NormalizedEmail = email.ToUpperInvariant();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the email address for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the email address.</returns>
    public Task<string?> GetEmailAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult<string?>(user.Email);
    }

    /// <summary>
    /// Gets a flag indicating whether the email address for a user has been confirmed.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the email confirmation status.</returns>
    public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.EmailConfirmed);
    }

    /// <summary>
    /// Sets a flag indicating whether the email address for a user has been confirmed.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="confirmed"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the normalized email address for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the normalized email address.</returns>
    public Task<string?> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.NormalizedEmail);
    }

    /// <summary>
    /// Sets the normalized email address for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="normalizedEmail"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetNormalizedEmailAsync(AppUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.NormalizedEmail = normalizedEmail?.ToUpperInvariant();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes a <see cref="UserLoginInfo"/> from a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="loginProvider"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task RemoveLoginAsync(AppUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        var userLogin = await context.UserLogins.FirstOrDefaultAsync(ul =>
            ul.UserId.ToString() == user.Id.ToString() &&
            ul.LoginProvider == loginProvider &&
            ul.ProviderKey == providerKey, cancellationToken);
        if (userLogin != null)
        {
            context.UserLogins.Remove(userLogin);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Gets the list of <see cref="UserLoginInfo"/>s associated with a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of <see cref="UserLoginInfo"/>s.</returns>
    public Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        var userId = user.Id.ToString();
        var userLogins = context.UserLogins
            .Where(ul => ul.UserId.ToString() == userId)
            .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey, ul.ProviderDisplayName))
            .ToList();
        return Task.FromResult<IList<UserLoginInfo>>(userLogins);
    }

    /// <summary>
    /// Finds a user by a login provider and provider key.
    /// </summary>
    /// <param name="loginProvider"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user associated with the login provider and provider key.</returns>
    public Task<AppUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(loginProvider, nameof(loginProvider));
        ArgumentNullException.ThrowIfNull(providerKey, nameof(providerKey));
        var userLogin = context.UserLogins
            .FirstOrDefault(ul => ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey);
        if (userLogin != null)
        {
            return context.Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == userLogin.UserId, cancellationToken);
        }
        return Task.FromResult<AppUser?>(null);
    }

    /// <summary>
    /// Sets the password hash for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="passwordHash"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetPasswordHashAsync(AppUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the password hash for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the password hash.</returns>
    public Task<string?> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.PasswordHash);
    }

    /// <summary>
    /// Gets a flag indicating whether the user has a password.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the password status.</returns>
    public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.PasswordHash != null);
    }

    /// <summary>
    /// Sets the phone number for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetPhoneNumberAsync(AppUser user, string? phoneNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.PhoneNumber = phoneNumber;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the phone number for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the phone number.</returns>
    public Task<string?> GetPhoneNumberAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.PhoneNumber);
    }

    /// <summary>
    /// Gets a flag indicating whether the phone number for a user has been confirmed.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the phone number confirmation status.</returns>
    public Task<bool> GetPhoneNumberConfirmedAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    /// <summary>
    /// Sets a flag indicating whether the phone number for a user has been confirmed.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="confirmed"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetPhoneNumberConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.PhoneNumberConfirmed = confirmed;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the security stamp for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="stamp"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetSecurityStampAsync(AppUser user, string stamp, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.SecurityStamp = stamp;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the security stamp for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the security stamp.</returns>
    public Task<string?> GetSecurityStampAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.SecurityStamp);
    }

    /// <summary>
    /// Gets the claims associated with a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of claims.</returns>
    public async Task<IList<Claim>> GetClaimsAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        var claims = await context.UserClaims
                                    .Where(uc => uc.UserId == user.Id.ToString())
                                    .ToListAsync(cancellationToken);
        return [.. claims
            .Where(uc => uc.ClaimType != null && uc.ClaimValue != null)
            .Select(uc => new Claim(uc.ClaimType!, uc.ClaimValue!))];
    }

    /// <summary>
    /// Replaces a claim for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="claim"></param>
    /// <param name="newClaim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task ReplaceClaimAsync(AppUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(claim, nameof(claim));
        ArgumentNullException.ThrowIfNull(newClaim, nameof(newClaim));

        var fo = context.UserClaims
            .Where(uc => uc.UserId == user.Id.ToString() && uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value)
            .ToList();
        foreach (var fc in fo)
        {
            fc.ClaimType = newClaim.Type;
            fc.ClaimValue = newClaim.Value;
        }
        context.UserClaims.UpdateRange(fo);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Removes the specified <paramref name="claims"/> from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="claims"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task RemoveClaimsAsync(AppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(claims, nameof(claims));
        var userClaims = context.UserClaims
            .Where(uc => uc.UserId == user.Id.ToString())
            .ToList();
        var claimsToRemove = userClaims
            .Where(uc => claims.Any(c => c.Type == uc.ClaimType && c.Value == uc.ClaimValue))
            .ToList();
        if (claimsToRemove.Count > 0)
        {
            context.UserClaims.RemoveRange(claimsToRemove);
            return context.SaveChangesAsync(cancellationToken);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the list of users associated with a specified <paramref name="claim"/>.
    /// </summary>
    /// <param name="claim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of users.</returns>
    public async Task<IList<AppUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(claim, nameof(claim));
        var userIds = context.UserClaims
            .Where(uc => uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value)
            .Select(uc => uc.UserId)
            .ToList();
        var users = await context.Users
            .Where(u => userIds.Contains(u.Id.ToString()))
            .ToListAsync(cancellationToken);
        return users;
    }

    public Task<DateTimeOffset?> GetLockoutEndDateAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.LockoutEnd);
    }

    /// <summary>
    /// Sets the lockout end date for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="lockoutEnd"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.LockoutEnd = lockoutEnd;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Increments the access failed count for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the new access failed count.</returns>
    public async Task<int> IncrementAccessFailedCountAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.AccessFailedCount++;
        await context.SaveChangesAsync(cancellationToken);
        return user.AccessFailedCount;
    }

    /// <summary>
    /// Resets the access failed count for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task ResetAccessFailedCountAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.AccessFailedCount = 0;
        return context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the access failed count for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the access failed count.</returns>
    public Task<int> GetAccessFailedCountAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.AccessFailedCount);
    }

    /// <summary>
    /// Gets a flag indicating whether the user can be locked out.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the lockout status.</returns>
    public Task<bool> GetLockoutEnabledAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.LockoutEnabled);
    }

    /// <summary>
    /// Sets a flag indicating whether the user can be locked out.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="enabled"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetLockoutEnabledAsync(AppUser user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.LockoutEnabled = enabled;
        return context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Adds the specified <paramref name="user"/> to the named <paramref name="roleName"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roleName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task AddToRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        var role = context.Roles.FirstOrDefault(r => r.Name == roleName);
        if (role == null)
            throw new InvalidOperationException($"Role '{roleName}' does not exist.");
        var userRole = new IdentityUserRole<string>
        {
            UserId = user.Id.ToString(),
            RoleId = role.Id
        };
        context.UserRoles.Add(userRole);
        return context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Removes the specified <paramref name="user"/> from the named <paramref name="roleName"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roleName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task RemoveFromRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        var role = context.Roles.FirstOrDefault(r => r.Name == roleName);
        if (role != null)
        {
            var userRole = context.UserRoles.FirstOrDefault(ur =>
                ur.UserId.ToString() == user.Id.ToString() && ur.RoleId == role.Id);
            if (userRole != null)
            {
                context.UserRoles.Remove(userRole);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }

    /// <summary>
    /// Gets the names of the roles the specified <paramref name="user"/> belongs to.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of role names.</returns>
    public async Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        var userId = user.Id.ToString();
        var roles = await (from ur in context.UserRoles
                           join r in context.Roles on ur.RoleId equals r.Id
                           where ur.UserId.ToString() == userId
                           select r.Name).ToListAsync(cancellationToken);
        return roles;
    }

    /// <summary>
    /// Gets a flag indicating whether the specified <paramref name="user"/> is a member of the named <paramref name="roleName"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roleName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role membership status.</returns>
    public async Task<bool> IsInRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        var userId = user.Id.ToString();
        var isInRole = await
            (from ur in context.UserRoles
             join r in context.Roles on ur.RoleId equals r.Id
             where ur.UserId.ToString() == userId && r.Name == roleName
             select r).AnyAsync(cancellationToken);
        return isInRole;
    }

    /// <summary>
    /// Gets the list of users who are members of the specified <paramref name="roleName"/>.
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of users.</returns>
    public async Task<IList<AppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        var users = await
            (from ur in context.UserRoles
             join r in context.Roles on ur.RoleId equals r.Id
             join u in context.Users on ur.UserId equals u.Id.ToString()
             where r.Name == roleName
             select u).ToListAsync(cancellationToken);
        return users;
    }

    /// <summary>
    /// Sets an authentication token for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="loginProvider"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task SetTokenAsync(AppUser user, string loginProvider, string name, string? value, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(loginProvider, nameof(loginProvider));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        var userToken = context.UserTokens.FirstOrDefault(ut =>
            ut.UserId.ToString() == user.Id.ToString() &&
            ut.LoginProvider == loginProvider &&
            ut.Name == name);
        if (userToken != null)
        {
            userToken.Value = value;
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Removes an authentication token for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="loginProvider"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task RemoveTokenAsync(AppUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(loginProvider, nameof(loginProvider));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        var userToken = context.UserTokens.FirstOrDefault(ut =>
            ut.UserId.ToString() == user.Id.ToString() &&
            ut.LoginProvider == loginProvider &&
            ut.Name == name);
        if (userToken != null)
        {
            context.UserTokens.Remove(userToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Gets an authentication token for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="loginProvider"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the token value.</returns>
    public async Task<string?> GetTokenAsync(AppUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(loginProvider, nameof(loginProvider));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        var userToken = await context.UserTokens.FirstOrDefaultAsync(ut =>
            ut.UserId.ToString() == user.Id.ToString() &&
            ut.LoginProvider == loginProvider &&
            ut.Name == name);
        return userToken?.Value;
    }

    /// <summary>
    /// Sets a flag indicating whether two-factor authentication is enabled for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="enabled"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task SetTwoFactorEnabledAsync(AppUser user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.TwoFactorEnabled = enabled;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a flag indicating whether two-factor authentication is enabled for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the two-factor authentication status.</returns>
    public Task<bool> GetTwoFactorEnabledAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return Task.FromResult(user.TwoFactorEnabled);
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
    public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        context.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    /// <summary>
    /// Deletes the specified <paramref name="user"/> from the user store.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public Task<AppUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(normalizedEmail, nameof(normalizedEmail));
        return context.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
    /// </returns>
    public async Task<AppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await context.Users.FindAsync([userId], cancellationToken);
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified normalized user name.
    /// </summary>
    /// <param name="normalizedUserName">The normalized user name to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName"/> if it exists.
    /// </returns>
    public async Task<AppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await context.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUserName, cancellationToken);
    }

    /// <summary>
    /// Gets the normalized user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the normalized user name for the specified <paramref name="user"/> if it exists.</returns>
    public Task<string?> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.NormalizedUserName);
    }

    /// <summary>
    /// Gets the user ID for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user ID for the specified <paramref name="user"/> if it exists.</returns>
    public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.Id.ToString());
    }

    /// <summary>
    /// Gets the user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user name for the specified <paramref name="user"/> if it exists.</returns>
    public Task<string?> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.UserName);
    }

    /// <summary>
    /// Sets the normalized name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="normalizedName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetNormalizedUserNameAsync(AppUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        user.NormalizedUserName = normalizedName?.ToUpperInvariant();
        return Task.CompletedTask;
    }


    /// <summary>
    /// Sets the user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetUserNameAsync(AppUser user, string? userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        user.UserName = userName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Updates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    /// <summary>
    /// Deletes the specified <paramref name="user"/> from the user store.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the delete operation.</returns>
    public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }
}

