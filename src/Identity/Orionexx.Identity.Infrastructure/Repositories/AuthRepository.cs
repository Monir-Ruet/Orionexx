using Microsoft.AspNetCore.Identity;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Application.Infrastructure.Repositories;

namespace Orionexx.Identity.Infrastructure.Repositories;

public class AuthRepository(SignInManager<AppUser> signInManager) : IAuthRepository
{
    public async Task<bool> LoginAsync(AppUser user, string password, CancellationToken cancellationToken = default)
    {
        var signInResult = await signInManager.PasswordSignInAsync(user, password, true, true);
        return signInResult.Succeeded;
    }
}