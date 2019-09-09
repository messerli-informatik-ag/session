using System;

namespace Bash.Session.Configuration
{
    public sealed class TimeoutSettings
    {
        public TimeSpan IdleTimeout { get; }

        public TimeSpan AbsoluteTimeout { get; }

        public TimeoutSettings(TimeSpan idleTimeout, TimeSpan absoluteTimeout)
        {
            IdleTimeout = idleTimeout;
            AbsoluteTimeout = absoluteTimeout;
        }
    }
}
