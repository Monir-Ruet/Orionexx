using MediatR;
using Microsoft.Extensions.Logging;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Identity.Application.Infrastructure.Repositories;

namespace Orionexx.Identity.Application.Handlers.Account.Command;

public class ForgotPasswordCommand : IRequest<Result>
{
    public required string Email { get; set; }
}

public class ForgotPassword(
    ILogger<ForgotPassword> logger,
    IAccountRepository accountRepository) : IRequestHandler<ForgotPasswordCommand, Result>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await accountRepository.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Failure("User not found");
            var resetCode = await accountRepository.GeneratePasswordResetTokenAsync(user);
            // Here you would typically send the token to the user's email.
            return Result.Success();
        }
        catch (Exception)
        {
            logger.LogError("An error occurred while processing forgot password request");
            return Result.Failure();
        }
    }
}