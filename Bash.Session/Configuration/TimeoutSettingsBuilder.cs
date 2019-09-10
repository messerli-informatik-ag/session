using System;

namespace Bash.Session.Configuration
{
    public sealed class TimeoutSettingsBuilder
    {
        private static readonly TimeSpan DefaultIdleTimeout = TimeSpan.FromDays(1);

        private static readonly TimeSpan DefaultAbsoluteTimeout = TimeSpan.FromDays(30);

        private readonly TimeSpan? _idleTimeout;

        private readonly TimeSpan? _absoluteTimeout;

        public TimeoutSettingsBuilder()
        {
        }

        private TimeoutSettingsBuilder(TimeSpan? idleTimeout, TimeSpan? absoluteTimeout)
        {
            _idleTimeout = idleTimeout;
            _absoluteTimeout = absoluteTimeout;
        }

        public TimeoutSettingsBuilder IdleTimeout(TimeSpan value)
            => ShallowClone(idleTimeout: value);

        public TimeoutSettingsBuilder AbsoluteTimeout(TimeSpan value)
            => ShallowClone(absoluteTimeout: value);

        public TimeoutSettings Build()
        {
            return new TimeoutSettings(
                _idleTimeout ?? DefaultIdleTimeout,
                _absoluteTimeout ?? DefaultAbsoluteTimeout);
        }

        private TimeoutSettingsBuilder ShallowClone(
            TimeSpan? idleTimeout = null,
            TimeSpan? absoluteTimeout = null)
        {
            return new TimeoutSettingsBuilder(
                idleTimeout ?? _idleTimeout,
                absoluteTimeout ?? _absoluteTimeout);
        }
    }
}
