using System.ComponentModel.DataAnnotations;

namespace Orionexx.Messaging.Configurations;

public class AppConfiguration : IAppConfiguration
{
    [Required] public AwsSesSettings AwsSesSettings { get; init; } = null!;
    IAwsSesSettings IAppConfiguration.AwsSesSettings => AwsSesSettings;
}

public interface IAwsSesSettings
{
    public string? FromEmail { get; init; }
    public string? FromName { get; init; }
    public string? AccessKey { get; init; }
    public string? SecretKey { get; init; }
}

public interface IAppConfiguration
{
    IAwsSesSettings AwsSesSettings { get; }
}