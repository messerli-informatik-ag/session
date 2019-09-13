using System;
using Messerli.Session.Configuration;
using Messerli.Session.Internal;
using Messerli.Session.SessionState;
using Messerli.Session.Storage;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public sealed class ExpirationRetrieverTest
    {
        private static readonly SessionId SessionId = new SessionId("foo-bar");

        [Theory]
        [MemberData(nameof(TestData))]
        public void TestIdleExpirationIsCalculatedCorrectly(DateTime today, TimeSpan idleTimeout, DateTime expectedExpiration)
        {
            var dateTimeFactory = new Mock<IDateTimeFactory>();
            dateTimeFactory.Setup(f => f.Now())
                .Returns(today);
            var settings = new TimeoutSettingsBuilder()
                .IdleTimeout(idleTimeout)
                .AbsoluteTimeout(TimeSpan.FromDays(10000))
                .Build();
            var rawSession = CreateRawSession(today);

            var expirationRetriever = new ExpirationRetriever(dateTimeFactory.Object, settings);
            Assert.Equal(expectedExpiration, expirationRetriever.GetExpiration(rawSession));
        }

        [Fact]
        public void ReturnsAbsoluteExpirationIfAbsoluteExpirationIsCloserToNow()
        {
            var creationDate = new DateTime(year: 2000, month: 1, day: 1);
            var idleTimeout = TimeSpan.FromDays(5);
            var absoluteTimeout = TimeSpan.FromDays(10);
            var today = new DateTime(year: 2000, month: 1, day: 8);
            var expectedExpiration = new DateTime(year: 2000, month: 1, day: 11);

            var session = CreateRawSession(creationDate);
            var settings = new TimeoutSettingsBuilder()
                .IdleTimeout(idleTimeout)
                .AbsoluteTimeout(absoluteTimeout)
                .Build();

            var dateTimeFactory = new Mock<IDateTimeFactory>();
            dateTimeFactory.Setup(f => f.Now())
                .Returns(today);

            var expirationRetriever = new ExpirationRetriever(dateTimeFactory.Object, settings);
            Assert.Equal(expectedExpiration, expirationRetriever.GetExpiration(session));
        }

        [Fact]
        public void ReturnsIdleExpirationIfAbsoluteExpirationIsCloserToNow()
        {
            var creationDate = new DateTime(year: 2000, month: 1, day: 1);
            var idleTimeout = TimeSpan.FromDays(5);
            var absoluteTimeout = TimeSpan.FromDays(10);
            var today = new DateTime(year: 2000, month: 1, day: 2);
            var expectedExpiration = new DateTime(year: 2000, month: 1, day: 7);

            var session = CreateRawSession(creationDate);
            var settings = new TimeoutSettingsBuilder()
                .IdleTimeout(idleTimeout)
                .AbsoluteTimeout(absoluteTimeout)
                .Build();

            var dateTimeFactory = new Mock<IDateTimeFactory>();
            dateTimeFactory.Setup(f => f.Now())
                .Returns(today);

            var expirationRetriever = new ExpirationRetriever(dateTimeFactory.Object, settings);
            Assert.Equal(expectedExpiration, expirationRetriever.GetExpiration(session));
        }

        private static RawSession CreateRawSession(DateTime creationDate)
        {
            return new RawSession(new New(SessionId), new SessionData(creationDate));
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
