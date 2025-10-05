using MediatR;
using Microsoft.AspNetCore.Identity;
using Orionexx.Proto;
using Microsoft.Extensions.Logging;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Application.Utilities;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Application.Handlers.Auth.Query;

public class LoginQuery : IRequest<Result<AccessTokenResponse>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class Login(
    ILogger<Login> logger,
    IAuthRepository authRepository,
    UserManager<AppUser> userManager,
    ITokenProvider tokenProvider) : IRequestHandler<LoginQuery, Result<AccessTokenResponse>>
{
    public async Task<Result<AccessTokenResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return (Result<AccessTokenResponse>)Result.Failure("User not found");

            var isLoggedIn = await authRepository.LoginAsync(user, request.Password, cancellationToken);

            if (!isLoggedIn)
                return Result.Failure<AccessTokenResponse>();

            var accessToken = tokenProvider.GenerateToken(user.Id.ToString(), user.Email, "user", "", true);
            var refreshToken = tokenProvider.GenerateToken(user.Id.ToString(), user.Email, "user", "", false);

            return Result.Success(new AccessTokenResponse
            {
                AccessToken = accessToken,
                ExpiresIn = 15 * 60,
                TokenType = "Bearer",
                RefreshToken = refreshToken
            });
        }
        catch (Exception)
        {
            logger.LogError("An error occurred while logging in user");
            return Result.Failure<AccessTokenResponse>();
        }
    }
}