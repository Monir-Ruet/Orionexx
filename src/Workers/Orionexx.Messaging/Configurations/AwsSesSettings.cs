namespace Orionexx.Messaging.Configurations;

public class AwsSesSettings : IAwsSesSettings
{
    public string? FromEmail { get; init; }
    public string? FromName { get; init; }
    public string? AccessKey { get; init; }
    public string? SecretKey { get; init; }
}
