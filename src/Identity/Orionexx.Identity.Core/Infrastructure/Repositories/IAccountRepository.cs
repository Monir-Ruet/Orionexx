using Microsoft.AspNetCore.Identity;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Core.Infrastructure.Repositories;

public interface IAccountRepository
{
    Task<IdentityResult> RegisterAsync(AppUser request, string password);
    Task<AppUser?> FindByEmailAsync(string email);
    Task<bool> ResetPasswordAsync(AppUser user, string resetCode, string newPassword);
    Task<string> GeneratePasswordResetTokenAsync(AppUser user);
}