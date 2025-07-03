using Orionexx.Identity.Grpc.Services;

namespace Orionexx.Identity.Grpc.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        app.MapGrpcReflectionService();
        app.MapGrpcService<IdentityService>();
        app.UseCloudEvents();
        app.MapSubscribeHandler();
        
        return app;
    }
}