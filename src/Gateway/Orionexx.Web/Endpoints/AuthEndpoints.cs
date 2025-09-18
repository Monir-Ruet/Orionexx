using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Orionexx.Proto;
using Orionexx.Web.DTOs.Identity;
using Orionexx.Web.Filters;

namespace Orionexx.Web.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("/auth").WithTags("Auth");

        route.MapPost("/login", Login)
            .AddEndpointFilter<ValidationFilter<LoginRequestDto>>()
            .Produces<AccessTokenResponse>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequestDto loginRequest,
        IMapper mapper,
        Auth.AuthClient identityGrpcClient)
    {
        try
        {
            var request = mapper.Map<LoginRequest>(loginRequest);
            var response = await identityGrpcClient.LoginAsync(request);
            return response is null ? Results.BadRequest() : Results.Ok(response);
        }
        catch (RpcException ex)
        {
            return Results.Problem(statusCode: StatusCodes.Status401Unauthorized, detail: ex.Status.Detail);
        }
    }
}