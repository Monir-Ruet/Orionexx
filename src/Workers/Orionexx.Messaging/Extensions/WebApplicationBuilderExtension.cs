using Amazon;
using Amazon.SimpleEmail;
using Orionexx.Messaging.Configurations;
using Orionexx.Messaging.Services;

namespace Orionexx.Messaging.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var appConfiguration = new AppConfiguration();
        builder.Configuration.Bind(appConfiguration);

        builder.Services.AddSingleton<IAppConfiguration>(appConfiguration);
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<IMessagingService, MessagingService>();
        builder.Services.AddScoped<ISesEmailService, SesEmailService>();
        builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();

        builder.Services.AddScoped<IAmazonSimpleEmailService>(_ =>
        {
            var accessKey = appConfiguration.AwsSesSettings.AccessKey;
            var secretKey = appConfiguration.AwsSesSettings.SecretKey;

            return new AmazonSimpleEmailServiceClient(accessKey, secretKey, RegionEndpoint.CACentral1);
        });

        return builder;
    }
}