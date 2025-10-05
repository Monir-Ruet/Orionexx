using Orionexx.Messaging.EventsHandler;

namespace Orionexx.Messaging.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseCloudEvents();
        app.MapSubscribeHandler();

        var routeGroup = app.MapGroup("events");
        routeGroup.HandleAccountEvents();

        return app;
    }
}