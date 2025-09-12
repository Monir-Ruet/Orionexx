using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Orionexx.Proto;
using Orionexx.Web.DTOs.Account;
using Orionexx.Web.DTOs.Identity;
using Orionexx.Web.Filters;
using Orionexx.Web.Validators;

namespace Orionexx.Web.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("/account").WithTags("Account");

        route.MapPost("/profile", Profile)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        route.MapGet("/ForgotPassword", ForgotPassword)
            .Produces(StatusCodes.Status200OK);

        route.MapPost("/ResetPassword", ResetPassword)
            .AddEndpointFilter<ValidationFilter<ResetPasswordRequestDto>>()
            .Produces(StatusCodes.Status200OK);

        route.MapPost("/RefreshAccessToken", RefreshAccessToken)
            .AddEndpointFilter<ValidationFilter<RefreshAccessTokenRequestDto>>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        route.MapPost("/signup", Signup)
            .AddEndpointFilter<ValidationFilter<SignupRequestDto>>()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> Signup(
        [FromBody] SignupRequestDto req,
        IMapper mapper,
        Account.AccountClient accountGrpcClient)
    {
        try
        {
            var request = mapper.Map<SignupRequest>(req);
            await accountGrpcClient.SignupAsync(request);
            return Results.Created();
        }
        catch (RpcException ex)
        {
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: ex.Status.Detail);
        }
    }

    private static async Task<IResult> Profile(
        [FromQuery, Required] string email,
        IMapper mapper,
        Account.AccountClient accountGrpcClient)
    {
        if (ValidatorsHelpers.IsValidEmail(email))
            return Results.BadRequest();
        var profileReq = new ProfileRequest()
        {
            Email = email
        };
        var profile = await accountGrpcClient.ProfileAsync(profileReq);
        var user = mapper.Map<UserResponseDto>(profile);
        return Results.Ok(user);
    }

    private static async Task<IResult> ForgotPassword(
        [FromQuery, Required] string email,
        Account.AccountClient accountGrpcClient)
    {
        try
        {
            if (ValidatorsHelpers.IsValidEmail(email))
                return Results.BadRequest();
            var request = new ForgotPasswordRequest()
            {
                Email = email
            };
            await accountGrpcClient.ForgotPasswordAsync(request);
            return Results.Ok();
        }
        catch
        {
            return Results.BadRequest();
        }

    }

    private static async Task<IResult> ResetPassword(
        [FromBody] ResetPasswordRequestDto resetPasswordRequest,
        IMapper mapper,
        Account.AccountClient accountGrpcClient)
    {
        try
        {
            var request = mapper.Map<ResetPasswordRequest>(resetPasswordRequest);
            await accountGrpcClient.ResetPasswordAsync(request);
            return Results.Ok();
        }
        catch (RpcException)
        {
            return Results.BadRequest();
        }
    }

    private static async Task<IResult> RefreshAccessToken(
        [FromBody] RefreshAccessTokenRequestDto resetPasswordRequest,
        IMapper mapper,
        Account.AccountClient accountGrpcClient)
    {
        try
        {
            var request = mapper.Map<ResetPasswordRequest>(resetPasswordRequest);
            await accountGrpcClient.ResetPasswordAsync(request);
            return Results.Ok();
        }
        catch (RpcException)
        {
            return Results.BadRequest();
        }
    }
}