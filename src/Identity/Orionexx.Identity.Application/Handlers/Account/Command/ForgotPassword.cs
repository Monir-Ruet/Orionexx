using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Application.Handlers.Account.Command;

public class ForgotPasswordCommand : IRequest<Result>
{
    public required string Email { get; set; }
}

public class ForgotPassword(
    ILogger<ForgotPassword> logger,
    UserManager<AppUser> userManager) : IRequestHandler<ForgotPasswordCommand, Result>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Failure("User not found");
            var resetCode = await userManager.GeneratePasswordResetTokenAsync(user);
            return Result.Success();
        }
        catch (Exception)
        {
            logger.LogError("An error occurred while processing forgot password request");
            return Result.Failure();
        }
    }
}