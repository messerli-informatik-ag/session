using System;

namespace Bash.AspNetCore.Session
{
    public class SessionConfiguration
    {
        public TimeSpan IdleTimeout { get; }

        public TimeSpan AbsoluteTimeout { get; }
    }
}
