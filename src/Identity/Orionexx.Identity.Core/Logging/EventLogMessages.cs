using Microsoft.Extensions.Logging;

namespace Orionexx.Identity.Core.Logging;

public static partial class EventLogMessages
{
    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = "No events to process")]
    public static partial void NoEventsToProcess(this ILogger logger);

    [LoggerMessage(
        EventId = 4004,
        Level = LogLevel.Information,
        Message = "Event {EventId} of type {EventType} published successfully")]
    public static partial void EventPublishedSuccessfully(this ILogger logger, long eventId, string eventType);

    [LoggerMessage(
        EventId = 4005,
        Level = LogLevel.Error,
        Message = "Failed to publish event {EventId} of type {EventType}")]
    public static partial void EventPublishFailed(this ILogger logger, long eventId, string eventType, Exception ex);

    [LoggerMessage(
        EventId = 4006,
        Level = LogLevel.Warning,
        Message = "Event {EventId} exceeded maximum retry attempts")]
    public static partial void EventMaxRetriesExceeded(this ILogger logger, long eventId);
}
