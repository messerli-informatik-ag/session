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
            return _dateTimeFactory.Now() + _timeoutSettings.IdleTimeout;
        }
    }
}
