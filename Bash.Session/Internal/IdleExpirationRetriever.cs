using System;
using Bash.Session.Configuration;

namespace Bash.Session.Internal
{
    internal class IdleExpirationRetriever : IIdleExpirationRetriever
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        private readonly TimeoutSettings _timeoutSettings;

        public IdleExpirationRetriever(
            IDateTimeFactory dateTimeFactory,
            TimeoutSettings timeoutSettings)
        {
            _dateTimeFactory = dateTimeFactory;
            _timeoutSettings = timeoutSettings;
        }

        public DateTime GetIdleExpiration() => _dateTimeFactory.Now() + _timeoutSettings.IdleTimeout;
    }
}
