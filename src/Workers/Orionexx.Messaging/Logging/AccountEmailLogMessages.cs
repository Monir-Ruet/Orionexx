namespace Orionexx.Messaging.Logging;

public static partial class AccountEventLogMessaging
{
    [LoggerMessage(
        EventId = 6001,
        Level = LogLevel.Error,
        Message = "Account create confirmation email send failure")]
    public static partial void AccountCreattionFailure(this ILogger logger, Exception ex);
}