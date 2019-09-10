using System;
using Bash.Session.Configuration;
using Xunit;

namespace Bash.Session.Test.Configuration
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
