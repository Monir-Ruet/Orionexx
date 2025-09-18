using System.Reflection;
using FluentValidation;
using Orionexx.Identity.Application;
using Orionexx.Identity.Infrastructure;
using Orionexx.ServiceDefaults;

namespace Orionexx.Identity.Service.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureWebApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();
        builder.Services.AddDaprClient();
        builder.AddServiceDefaults();
        builder.AddDefaultAuthentication();
        builder.ConfigureApplication();
        builder.ConfigureInfrastructure();

        return builder;
    }
}