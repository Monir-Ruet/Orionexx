using Orionexx.Identity.Service.Services;

namespace Orionexx.Identity.Service.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        app.MapGrpcReflectionService();
        app.MapGrpcService<AuthService>();
        app.MapGrpcService<AccountService>();
        app.UseCloudEvents();
        app.MapSubscribeHandler();

        return app;
    }
}