using Microsoft.AspNetCore.Identity;
using Orionexx.Identity.Application.Handlers.Account.Command;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Infrastructure.Repositories;

public class AccountRepository(UserManager<AppUser> userManager) : IAccountRepository
{
    public async Task<bool> RegisterAsync(RegisterCommand request)
    {
        var user = new AppUser()
        {
            Email = request.Email,
            UserName = request.Email
        };
        var result = await userManager.CreateAsync(user, request.Password);
        return result.Succeeded;
    }

    public async Task<AppUser?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }
}