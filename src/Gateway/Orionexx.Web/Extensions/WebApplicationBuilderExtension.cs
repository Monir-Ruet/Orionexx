using System.Reflection;
using Asp.Versioning;
using FluentValidation;
using Orionexx.Core.Shared.Constants;
using Orionexx.Proto;
using Orionexx.ServiceDefaults;

namespace Orionexx.Web.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        builder.AddServiceDefaults();

        builder.Services.AddDaprClient();

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.AddDaprGrpcClient<Account.AccountClient>(ServiceInvocations.IdentityService);
        builder.AddDaprGrpcClient<Auth.AuthClient>(ServiceInvocations.IdentityService);

        return builder;
    }
}