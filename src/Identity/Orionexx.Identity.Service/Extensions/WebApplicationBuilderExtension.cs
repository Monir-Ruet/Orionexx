using System.Reflection;
using FluentValidation;
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
        builder.ConfigureInfrastructure();

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return builder;
    }
}