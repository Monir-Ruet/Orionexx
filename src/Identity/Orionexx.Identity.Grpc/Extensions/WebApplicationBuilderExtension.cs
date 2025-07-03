using Orionexx.ServiceDefaults;

namespace Orionexx.Identity.Grpc.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureWebApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();
        builder.Services.AddDaprClient();
        builder.AddServiceDefaults();

        return builder;
    }
}