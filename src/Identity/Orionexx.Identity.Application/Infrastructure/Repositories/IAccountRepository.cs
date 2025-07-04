using Orionexx.Identity.Application.Handlers.Account.Command;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Application.Infrastructure.Repositories;

public interface IAccountRepository
{
    Task<bool> RegisterAsync(RegisterCommand request);
    Task<AppUser?> FindByEmailAsync(string email);
}