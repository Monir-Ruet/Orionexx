using Grpc.Core;
using MediatR;
using Orionexx.Identity.Application.Handlers.Auth.Query;
using Orionexx.Proto;

namespace Orionexx.Identity.Grpc.Services;

public class AuthService(IMediator mediator) : Auth.AuthBase
{
    public override async Task<AccessTokenResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var loginQuery = new LoginQuery
        {
            Email = request.Email,
            Password = request.Password
        };
        var result = await mediator.Send(loginQuery);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Authentication failed"));
        return result.Value;
    }
}