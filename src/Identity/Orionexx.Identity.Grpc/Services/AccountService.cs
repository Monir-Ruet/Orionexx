using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Orionexx.Identity.Application.Handlers.Account.Command;
using Orionexx.Identity.Application.Handlers.Account.Query;
using Orionexx.Proto;

namespace Orionexx.Identity.Grpc.Services;

public class AccountService(IMediator mediator) : Account.AccountBase
{
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
            throw new RpcException(new Status(StatusCode.AlreadyExists, "There is already an account with this credentials"));
        return new Empty();
    }

    public override async Task<User> Profile(ProfileRequest request, ServerCallContext context)
    {
        var profileQuery = new ProfileQuery
        {
            Email = request.Email
        };
        var profileResult = await mediator.Send(profileQuery);
        if (!profileResult.IsSuccess)
            throw new RpcException(new Status(StatusCode.NotFound, "There is no user with this credentials"));
        return profileResult.Value;
    }

    public override async Task<Empty> ResetPassword(ResetPasswordRequest request, ServerCallContext context)
    {
        var resetPasswordCommand = new ResetPasswordCommand
        {
            Email = request.Email,
            ResetCode = request.ResetCode,
            NewPassword = request.NewPassword
        };
        var isResetSuccess = await mediator.Send(resetPasswordCommand);
        if (!isResetSuccess.Succeeded)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Password reset failed"));
        return await Task.FromResult(new Empty());
    }

    public override async Task<Empty> ForgotPassword(ForgotPasswordRequest request, ServerCallContext context)
    {
        var forgotPasswordCommand = new ForgotPasswordCommand
        {
            Email = request.Email
        };
        var forgotPasswordResult = await mediator.Send(forgotPasswordCommand);
        if (!forgotPasswordResult.IsSuccess)
            throw new RpcException(new Status(StatusCode.NotFound, "There is no user with this credentials"));
        return await Task.FromResult(new Empty());
    }
}