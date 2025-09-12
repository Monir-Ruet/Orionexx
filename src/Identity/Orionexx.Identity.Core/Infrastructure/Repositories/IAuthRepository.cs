using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Core.Infrastructure.Repositories;

public interface IAuthRepository
{
    Task<bool> LoginAsync(AppUser user, string password, CancellationToken cancellationToken = default);
}