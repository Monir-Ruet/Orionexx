using AutoMapper;
using Grpc.Core;
using MediatR;
using Orionexx.Proto;
using Google.Protobuf.WellKnownTypes;
using Orionexx.Identity.Application.Handlers.Account.Command;
using Orionexx.Identity.Application.Handlers.Authentication.Query;

namespace Orionexx.Identity.Grpc.Services;

public class IdentityService(
    IMediator mediator,
    IMapper mapper) : Proto.Identity.IdentityBase
{
    public override async Task<AccessTokenResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var loginQuery = new LoginQuery()
        {
            Email = request.Email,
            Password = request.Password
        };
        var result = await mediator.Send(loginQuery);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Login failed"));
        return mapper.Map<AccessTokenResponse>(result.Value);
    }

    public override async Task<Empty> Signup(SignupRequest request, ServerCallContext context)
    {
        var registerCommand = new RegisterCommand
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
        var registerResult = await mediator.Send(registerCommand);
        if (!registerResult.IsSuccess)
            throw new RpcException(new Status(StatusCode.FailedPrecondition, "Sign up failed"));
        return new Empty();
    }
}