using Asp.Versioning;
using Orionexx.Web.Endpoints;

namespace Orionexx.Web.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        app.UseHttpsRedirection();

        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        var routeGroup = app
            .MapGroup("api/v{api-version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        routeGroup.MapIdentityEndpoints();

        return app;
    }
}