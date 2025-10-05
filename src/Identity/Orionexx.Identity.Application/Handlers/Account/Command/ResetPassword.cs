using MediatR;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Application.Interfaces.Repositories;

namespace Orionexx.Identity.Application.Handlers.Account.Command;

public class ResetPasswordCommand : IRequest<bool>
{
    public required string Email { get; set; }
    public required string ResetCode { get; set; }
    public required string NewPassword { get; set; }
}

public class ResetPasswordCommandHandler(IAccountRepository accountRepository) : IRequestHandler<ResetPasswordCommand, bool>
{
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await accountRepository.FindByEmailAsync(request.Email);
        if (user is null)
            return false;
        return await accountRepository.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
    }
}