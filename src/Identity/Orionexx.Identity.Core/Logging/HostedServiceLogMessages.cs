using Microsoft.Extensions.Logging;

namespace Orionexx.Identity.Core.Logging;

public static partial class HostedServiceLogMessages
{
    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Information,
        Message = "Timed Hosted Service started")]
    public static partial void StartHostedService(this ILogger logger);

    [LoggerMessage(
        EventId = 5002,
        Level = LogLevel.Information,
        Message = "Timed Hosted Service stopped")]
    public static partial void StopHostedService(this ILogger logger);

    [LoggerMessage(
        EventId = 5003,
        Level = LogLevel.Error,
        Message = "Failed to process events batch")]
    public static partial void BatchProcessingError(this ILogger logger, Exception ex);
}