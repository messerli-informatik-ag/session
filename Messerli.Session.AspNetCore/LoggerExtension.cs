using System;
using Microsoft.Extensions.Logging;

namespace Messerli.Session.AspNetCore
{
    internal static class LoggerExtension
    {
        private static readonly Action<ILogger, Exception> ErrorSavingTheSessionLoggerMessage =
            LoggerMessage.Define(
                eventId: new EventId(1, "ErrorSavingTheSession"),
                logLevel: LogLevel.Error,
                formatString: "Error saving the session.");

        public static void ErrorSavingTheSession(this ILogger logger, Exception exception)
        {
            ErrorSavingTheSessionLoggerMessage(logger, exception);
        }
    }
}
