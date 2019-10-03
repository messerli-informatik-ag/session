#pragma warning disable 660,661

using System;

namespace Messerli.Session.Configuration
{
    [Equals]
    public sealed class TimeoutSettings
    {
        internal TimeoutSettings(TimeSpan idleTimeout, TimeSpan absoluteTimeout)
        {
            IdleTimeout = idleTimeout;
            AbsoluteTimeout = absoluteTimeout;
        }

        public TimeSpan IdleTimeout { get; }

        public TimeSpan AbsoluteTimeout { get; }

        public static bool operator ==(TimeoutSettings left, TimeoutSettings right) => Operator.Weave(left, right);

        public static bool operator !=(TimeoutSettings left, TimeoutSettings right) => Operator.Weave(left, right);
    }
}
