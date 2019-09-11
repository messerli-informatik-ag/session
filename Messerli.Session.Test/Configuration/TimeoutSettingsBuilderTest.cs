using System;
using Messerli.Session.Configuration;
using Xunit;

namespace Messerli.Session.Test.Configuration
{
    public class TimeoutSettingsBuilderTest
    {
        [Fact]
        public void BuilderHasDefaultValues()
        {
            var _ = new TimeoutSettingsBuilder().Build();
        }

        [Theory]
        [MemberData(nameof(InvalidTimeouts))]
        public void ThrowsIfIdleTimeoutIsInvalid(TimeSpan invalidTimeout)
        {
            var builder = new TimeoutSettingsBuilder().IdleTimeout(invalidTimeout);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var _ = builder.Build();
            });
        }

        [Theory]
        [MemberData(nameof(InvalidTimeouts))]
        public void ThrowsIfAbsoluteTimeoutIsInvalid(TimeSpan invalidTimeout)
        {
            var builder = new TimeoutSettingsBuilder().AbsoluteTimeout(invalidTimeout);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var _ = builder.Build();
            });
        }

        [Fact]
        public void UsesProvidedValues()
        {
            var idleTimeout = TimeSpan.FromDays(1);
            var absoluteTimeout = TimeSpan.FromDays(2);

            var expectedSettings = new TimeoutSettings(idleTimeout, absoluteTimeout);

            var settings = new TimeoutSettingsBuilder()
                .IdleTimeout(idleTimeout)
                .AbsoluteTimeout(absoluteTimeout)
                .Build();
            Assert.Equal(expectedSettings, settings);
        }

        public static TheoryData<TimeSpan> InvalidTimeouts()
        {
            var negativeTimeSpan = TimeSpan.FromTicks(-1);
            return new TheoryData<TimeSpan>
            {
                TimeSpan.Zero,
                negativeTimeSpan,
            };
        }
    }
}
