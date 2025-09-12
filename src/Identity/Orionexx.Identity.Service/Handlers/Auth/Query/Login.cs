using MediatR;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Identity.Core.Infrastructure.Repositories;
using Orionexx.Identity.Core.Infrastructure.Utilities;
using Orionexx.Proto;

namespace Orionexx.Identity.Service.Handlers.Auth.Query;

public class LoginQuery : IRequest<Result<AccessTokenResponse>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class Login(
    IAuthRepository authRepository,
    IAccountRepository accountRepository,
    ITokenProvider tokenProvider) : IRequestHandler<LoginQuery, Result<AccessTokenResponse>>
{
    public async Task<Result<AccessTokenResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await accountRepository.FindByEmailAsync(request.Email);
        if (user is null)
            return (Result<AccessTokenResponse>)Result.Failure("User not found");

        var isLoggedIn = await authRepository.LoginAsync(user, request.Password, cancellationToken);
        if (!isLoggedIn) return Result.Failure<AccessTokenResponse>();

        var accessToken = tokenProvider.GenerateToken(user.Id, user.Email!, "user", "", true);
        var refreshToken = tokenProvider.GenerateToken(user.Id, user.Email!, "user", "", false);

        return Result.Success(new AccessTokenResponse
        {
            AccessToken = accessToken,
            ExpiresIn = 15 * 60,
            RefreshToken = refreshToken
        });
    }
}