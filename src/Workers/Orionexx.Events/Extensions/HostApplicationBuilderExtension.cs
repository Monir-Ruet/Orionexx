using Microsoft.EntityFrameworkCore;
using Orionexx.Events.Data;
using Orionexx.Events.Services;

namespace Orionexx.Events.Extensions;

public static class HostApplicationBuilderExtension
{
    public static HostApplicationBuilder AddServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Orionexx"));
        });
        
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddDaprClient();
        builder.Services.AddScoped<IEventPublisher, EventPublisher>();
        builder.Services.AddScoped<IEventService, EventService>();
        
        return builder;
    }
}