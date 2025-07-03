using Microsoft.AspNetCore.Mvc;
using Orionexx.Proto;

namespace Orionexx.Web.Endpoints;

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("/identity");

        route.MapPost("/", Login)
            .HasApiVersion(1, 0)
            .WithDescription("Login with credentials")
            .WithDisplayName("Orionexx Identity")
            .WithTags("Identity")
            .Produces<LoginResponse>()
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        Identity.IdentityClient client)
    {
        var response = await client.LoginAsync(request);
        return Results.Ok(response);
    }
}