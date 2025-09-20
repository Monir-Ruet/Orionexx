using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orionexx.Identity.Application.Utilities;

namespace Orionexx.Identity.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder ConfigureApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddScoped<ITokenProvider, TokenProvider>();

        return builder;
    }
}