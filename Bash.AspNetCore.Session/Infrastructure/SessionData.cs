using System;
using System.Collections.Generic;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public class SessionData
    {
        public DateTime Created { get; }

        public TimeSpan IdleTimeout { get; }

        public TimeSpan AbsoluteTimeout { get; }

        public IDictionary<string, string> Data { get; }

        public SessionData(DateTime created,
            TimeSpan idleTimeout,
            TimeSpan absoluteTimeout,
            IDictionary<string, string> data)
        {
            Created = created;
            IdleTimeout = idleTimeout;
            AbsoluteTimeout = absoluteTimeout;
            Data = data;
        }
    }
}
