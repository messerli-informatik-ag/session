using System;
using System.Diagnostics.Contracts;

namespace Messerli.Session.Configuration
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

        [Pure]
        public TimeoutSettingsBuilder IdleTimeout(TimeSpan value) => ShallowClone(idleTimeout: value);

        [Pure]
        public TimeoutSettingsBuilder AbsoluteTimeout(TimeSpan value) => ShallowClone(absoluteTimeout: value);

        [Pure]
        public TimeoutSettings Build()
        {
            var idleTimeout = _idleTimeout ?? DefaultIdleTimeout;
            var absoluteTimeout = _absoluteTimeout ?? DefaultAbsoluteTimeout;

            ValidateTimeout(idleTimeout, nameof(TimeoutSettings.IdleTimeout));
            ValidateTimeout(absoluteTimeout, nameof(TimeoutSettings.AbsoluteTimeout));

            if (absoluteTimeout < idleTimeout)
            {
                throw new InvalidOperationException("Absolute timeout must be greater than or equal to the idle timeout");
            }

            return new TimeoutSettings(idleTimeout, absoluteTimeout);
        }

        private static void ValidateTimeout(TimeSpan timeout, string propertyName)
        {
            if (timeout <= TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{propertyName} is not allowed to be negative or zero.");
            }
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
