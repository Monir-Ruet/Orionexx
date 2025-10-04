namespace Orionexx.Events.Logging;

public static partial class HostedServiceLogMessages
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Timed Hosted Service started")]
    public static partial void StartHostedService(this ILogger logger);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Timed Hosted Service stopped")]
    public static partial void StopHostedService(this ILogger logger);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message = "Failed to process events batch")]
    public static partial void BatchProcessingError(this ILogger logger, Exception ex);
}