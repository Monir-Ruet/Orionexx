using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orionexx.Identity.Application.Infrastructure.Configurations;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Infrastructure.Configurations;
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
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(appConfiguration.ConnectionStrings.Orionexx);
        });

        builder.Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        });

        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();

        return builder;
    }
}
