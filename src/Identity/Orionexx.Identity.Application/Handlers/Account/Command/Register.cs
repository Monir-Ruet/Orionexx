using MediatR;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Identity.Application.Infrastructure.Repositories;

namespace Orionexx.Identity.Application.Handlers.Account.Command;


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
        var isRegistered = await accountRepository.RegisterAsync(request);
        return isRegistered ? Result.Success() : Result.Failure();
    }
}