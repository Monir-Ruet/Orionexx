using MediatR;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Core.Infrastructure.Repositories;

namespace Orionexx.Identity.Service.Handlers.Account.Command;


public class RegisterCommand : IRequest<Result>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class Register(IAccountRepository accountRepository) : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var appUser = new AppUser()
        {
            Email = request.Email,
            FirstName = request.Name,
        };
        var isRegistered = await accountRepository.RegisterAsync(appUser, request.Password);
        return isRegistered.Succeeded ? Result.Success() : Result.Failure();
    }
}