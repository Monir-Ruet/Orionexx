using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Orionexx.Proto;
using Orionexx.Web.Contracts.Identity;
using Orionexx.Web.Filters;

namespace Orionexx.Web.Endpoints;

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("/Identity");

        route.MapPost("/login", Login)
            .HasApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<LoginContract>>()
            .WithTags("identity")
            .Produces<AccessTokenResponse>()
            .Produces(StatusCodes.Status404NotFound);

        route.MapPost("/signup", Signup)
            .HasApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<SignupContract>>()
            .WithTags("identity")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> Login(
        [FromBody] LoginContract req,
        IMapper mapper,
        Identity.IdentityClient client)
    {
        try
        {
            var request = mapper.Map<LoginRequest>(req);
            var response = await client.LoginAsync(request);
            return response is null ? Results.BadRequest() : Results.Ok(response);
        }
        catch (RpcException)
        {
            return Results.Problem(statusCode: StatusCodes.Status401Unauthorized);
        }
    }

    private static async Task<IResult> Signup(
        [FromBody] SignupContract req,
        IMapper mapper,
        Identity.IdentityClient client)
    {
        try
        {
            var request = mapper.Map<SignupRequest>(req);
            var response = await client.SignupAsync(request);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}