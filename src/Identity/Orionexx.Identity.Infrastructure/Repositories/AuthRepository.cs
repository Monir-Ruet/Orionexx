using Microsoft.AspNetCore.Identity;
using Orionexx.Identity.Application.Handlers.Authentication.Query;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Infrastructure.Repositories;

public class AuthenticationRepository(
    SignInManager<AppUser> signInManager) : IAuthenticationRepository
{
    public async Task<bool> LoginAsync(AppUser user, string password, CancellationToken cancellationToken = default)
    {
        var signInResult = await signInManager.PasswordSignInAsync(user, password, true, true);
        return signInResult.Succeeded;
    }
}