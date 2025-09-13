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

public class Register(
    ILogger<Register> logger,
    IAccountRepository accountRepository) : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await accountRepository.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                return Result.Failure("User already exists");
            var appUser = new AppUser()
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.Name,
            };
            var isRegistered = await accountRepository.RegisterAsync(appUser, request.Password);
            return isRegistered.Succeeded ? Result.Success() : Result.Failure();
        }
        catch (Exception)
        {
            logger.LogError("An error occurred while registering user");
            return Result.Failure();
        }
    }
}