using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<bool> LoginAsync(AppUser user, string password, CancellationToken cancellationToken = default);
}