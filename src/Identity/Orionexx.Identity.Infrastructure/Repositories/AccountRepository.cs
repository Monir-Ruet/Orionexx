using Microsoft.AspNetCore.Identity;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Core.Infrastructure.Repositories;

namespace Orionexx.Identity.Infrastructure.Repositories;

public class AccountRepository(UserManager<AppUser> userManager) : IAccountRepository
{
    public async Task<IdentityResult> RegisterAsync(AppUser user, string password)
    {
        return await userManager.CreateAsync(user, password);
    }

    public async Task<AppUser?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }
}