using System;

namespace Bash.Session
{
    public class SessionConfiguration
    {
        public TimeSpan IdleTimeout { get; }

        public TimeSpan AbsoluteTimeout { get; }
    }
}
