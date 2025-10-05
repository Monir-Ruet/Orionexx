using MediatR;
using Microsoft.Extensions.Logging;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Application.Infrastructure;
using Orionexx.Identity.Application.Interfaces.Repositories;

namespace Orionexx.Identity.Application.Handlers.Account.Command;

public class RegisterCommand : IRequest<Result>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class Register(
    ILogger<Register> logger,
    IUnitOfWork unitOfWork,
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
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            var isRegistered = await accountRepository.RegisterAsync(appUser, request.Password);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return isRegistered.Succeeded ? Result.Success() : Result.Failure();
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while registering user {ex}", ex);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure();
        }
    }
}