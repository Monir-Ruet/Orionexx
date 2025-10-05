using Microsoft.AspNetCore.Identity;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Core.Events.Account;

namespace Orionexx.Identity.Infrastructure.Repositories;

public class AccountRepository(UserManager<AppUser> userManager) : IAccountRepository
{
    public async Task<IdentityResult> RegisterAsync(AppUser user, string password)
    {
        user.AddDomainEvent(new AccountCreated(user.Email));
        return await userManager.CreateAsync(user, password);
    }

    public async Task<AppUser?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> ResetPasswordAsync(AppUser user, string resetCode, string newPassword)
    {
        var result = await userManager.ResetPasswordAsync(user, resetCode, newPassword);
        return result.Succeeded;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
    {
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }
}