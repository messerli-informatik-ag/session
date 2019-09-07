using System;

namespace Bash.Session.Infrastructure
{
    public class IdleExpirationDateRetriever : IIdleExpirationDateRetriever
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        private readonly TimeoutSettings _timeoutSettings;

        public IdleExpirationDateRetriever(
            IDateTimeFactory dateTimeFactory,
            TimeoutSettings timeoutSettings)
        {
            _dateTimeFactory = dateTimeFactory;
            _timeoutSettings = timeoutSettings;
        }

        public DateTime GetIdleExpirationDate() => _dateTimeFactory.Now() + _timeoutSettings.IdleTimeout;
    }
}
