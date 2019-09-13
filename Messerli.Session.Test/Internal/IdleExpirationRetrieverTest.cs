using System;
using Messerli.Session.Configuration;
using Messerli.Session.Internal;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public sealed class IdleExpirationRetrieverTest
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void TestIdleExpirationIsCalculatedCorrectly(DateTime today, TimeSpan idleTimeout, DateTime expectedExpiration)
        {
            var dateTimeFactory = new Mock<IDateTimeFactory>();
            dateTimeFactory.Setup(f => f.Now())
                .Returns(today);
            var settings = new TimeoutSettingsBuilder()
                .IdleTimeout(idleTimeout)
                .Build();

            var idleExpirationRetriever = new ExpirationRetriever(dateTimeFactory.Object, settings);
            Assert.Equal(expectedExpiration, idleExpirationRetriever.GetExpiration());
        }

        public static TheoryData<DateTime, TimeSpan, DateTime> TestData()
        {
            const int year = 2019;
            const int month = 1;
            const int day = 1;
            const int hour = 0;
            const int minute = 0;
            const int second = 0;
            return new TheoryData<DateTime, TimeSpan, DateTime>
            {
                {
                    new DateTime(year, month, day, hour, minute, second),
                    TimeSpan.FromSeconds(1),
                    new DateTime(year, month, day, hour, minute, second + 1)
                },
                {
                    new DateTime(year, month, day, hour, minute, second),
                    TimeSpan.FromMinutes(1),
                    new DateTime(year, month, day, hour, minute + 1, second)
                },
                {
                    new DateTime(year, month, day, hour, minute, second),
                    TimeSpan.FromHours(1),
                    new DateTime(year, month, day, hour + 1, minute, second)
                },
                {
                    new DateTime(year, month, day, hour, minute, second),
                    TimeSpan.FromDays(1),
                    new DateTime(year, month, day + 1, hour, minute, second)
                },
            };
        }
    }
}
