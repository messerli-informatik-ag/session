using System;

namespace Bash.Session
{
    public sealed class TimeoutSettings
    {
        public TimeSpan IdleTimeout { get; }

        public TimeSpan AbsoluteTimeout { get; }
    }
}
