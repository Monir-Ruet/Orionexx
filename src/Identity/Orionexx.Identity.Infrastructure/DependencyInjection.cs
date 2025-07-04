using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Orionexx.Identity.Application.Infrastructure.Configurations;
using Orionexx.Identity.Application.Infrastructure.Repositories;
using Orionexx.Identity.Application.Infrastructure.Utilities;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Identity.Infrastructure.Configurations;
using Orionexx.Identity.Infrastructure.Data;
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
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Lockout.MaxFailedAccessAttempts = 5;
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidIssuer = "Orionexx.Identity",
                ValidAudience = "Orionexx.Web",
                IssuerSigningKey = new SymmetricSecurityKey("supersecretkey"u8.ToArray())
            };
        });


        builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        
        return builder;
    }
}
