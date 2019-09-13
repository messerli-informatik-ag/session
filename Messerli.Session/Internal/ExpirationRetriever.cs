using System;
using Messerli.Session.Configuration;

namespace Messerli.Session.Internal
{
    internal class ExpirationRetriever : IExpirationRetriever
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        private readonly TimeoutSettings _timeoutSettings;

        public ExpirationRetriever(
            IDateTimeFactory dateTimeFactory,
            TimeoutSettings timeoutSettings)
        {
            _dateTimeFactory = dateTimeFactory;
            _timeoutSettings = timeoutSettings;
        }

        public DateTime GetExpiration(RawSession session)
        {
            var idleExpiration = _dateTimeFactory.Now() + _timeoutSettings.IdleTimeout;
            var absoluteExpiration = session.SessionData.CreationDate + _timeoutSettings.AbsoluteTimeout;
            return MinOf(absoluteExpiration, idleExpiration);
        }

        private static DateTime MinOf(DateTime dateOne, DateTime dateTwo)
        {
            return new DateTime(Math.Min(dateOne.Ticks, dateTwo.Ticks));
        }
    }
}
