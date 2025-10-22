using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Core.Shared.Events.Account;
using Orionexx.Identity.Application.Interfaces;
using Orionexx.Identity.Core.Entities.Account;

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
    UserManager<AppUser> userManager) : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                return Result.Failure("User already exists");
            var appUser = new AppUser()
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.Name,
            };
            var isRegistered = await userManager.CreateAsync(appUser, request.Password);
            if (!isRegistered.Succeeded)
                return Result.Failure("Registration failed");
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Failure("User not found");
            var confirmationUrl = await userManager.GeneratePasswordResetTokenAsync(user);
            user.AddDomainEvent(new AccountCreated(request.Email, request.Name.Split(" ").Last(), confirmationUrl));
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return isRegistered.Succeeded ? Result.Success() : Result.Failure();
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while registering user {ex}", ex);
            return Result.Failure();
        }
    }
}