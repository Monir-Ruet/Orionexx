using MediatR;
using Microsoft.AspNetCore.Identity;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Application.Handlers.Account.Command;

public class ResetPasswordCommand : IRequest<IdentityResult>
{
    public required string Email { get; set; }
    public required string ResetCode { get; set; }
    public required string NewPassword { get; set; }
}

public class ResetPasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ResetPasswordCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return IdentityResult.Failed();
        return await userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
    }
}