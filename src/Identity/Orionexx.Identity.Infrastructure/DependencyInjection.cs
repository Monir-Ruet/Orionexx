using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orionexx.Identity.Application.Infrastructure;
using Orionexx.Identity.Application.Infrastructure.Configurations;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Infrastructure.Configurations;
using Orionexx.Identity.Infrastructure.HostedService;
using Orionexx.Identity.Infrastructure.IdentityStores;
using Orionexx.Identity.Infrastructure.Messaging;
using Orionexx.Identity.Infrastructure.Persistence;
using Orionexx.Identity.Infrastructure.Persistence.Interceptors;
using Orionexx.Identity.Infrastructure.Repositories;
using Orionexx.Identity.Infrastructure.Utilities;

namespace Orionexx.Identity.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method For Infrastructure dependency injection
    /// </summary>
    /// <param name="builder"></param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigureInfrastructure(this IHostApplicationBuilder builder)
    {
        var appConfiguration = new AppConfiguration();
        builder.Configuration.Bind(appConfiguration);
        ValidateConfigurationHelper.ValidateSectionRecursive(appConfiguration);

        builder.Services.AddSingleton<IAppConfiguration>(appConfiguration);
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(appConfiguration.ConnectionStrings.Orionexx);
            options.AddInterceptors(serviceProvider.GetRequiredService<ISaveChangesInterceptor>());
        });

        builder.Services.AddScoped<IUserStore<AppUser>, UserStore>();
        builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore>();

        builder.Services.AddIdentity<AppUser, IdentityRole>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityCore<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        });

        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddHostedService<EventHostedService>();
        builder.Services.AddScoped<IPublisher, Publisher>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        return builder;
    }
}
