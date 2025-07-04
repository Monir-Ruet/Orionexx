using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Application.Infrastructure.Repositories;

public interface IAuthenticationRepository
{
    Task<bool> LoginAsync(AppUser user, string password, CancellationToken cancellationToken = default);
}