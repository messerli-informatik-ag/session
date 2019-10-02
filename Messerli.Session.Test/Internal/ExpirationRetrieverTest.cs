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
        public void ReturnsCorrectExpiration(TestParameters parameters)
        {
            var session = CreateRawSession(parameters.CreationDate);
            var settings = new TimeoutSettingsBuilder()
                .IdleTimeout(parameters.IdleTimeout)
                .AbsoluteTimeout(parameters.AbsoluteTimeout)
                .Build();

            var dateTimeFactory = new Mock<IDateTimeFactory>();
            dateTimeFactory.Setup(f => f.Now())
                .Returns(parameters.Today);

            var expirationRetriever = new ExpirationRetriever(dateTimeFactory.Object, settings);
            Assert.Equal(parameters.ExpectedExpiration, expirationRetriever.GetExpiration(session));
        }

        public static TheoryData<TestParameters> TestData()
        {
            const int year = 2000;
            const int month = 1;
            var creationDate = new DateTime(year: year, month: month, day: 1);
            var idleTimeout = TimeSpan.FromDays(5);
            var absoluteTimeout = TimeSpan.FromDays(10);

            var idleExpirationIsCloserToToday =
                new TestParameters(
                    creationDate,
                    idleTimeout,
                    absoluteTimeout,
                    today: new DateTime(year: year, month: month, day: 2),
                    expectedExpiration: new DateTime(year: year, month: month, day: 7));

            var absoluteExpirationIsCloserToToday =
                new TestParameters(
                    creationDate,
                    idleTimeout,
                    absoluteTimeout,
                    today: new DateTime(year: year, month: month, day: 8),
                    expectedExpiration: new DateTime(year: year, month: month, day: 11));

            return new TheoryData<TestParameters>
            {
                idleExpirationIsCloserToToday,
                absoluteExpirationIsCloserToToday,
            };
        }

        public class TestParameters
        {
            public DateTime CreationDate { get; }

            public TimeSpan IdleTimeout { get; }

            public TimeSpan AbsoluteTimeout { get; }

            public DateTime Today { get; }

            public DateTime ExpectedExpiration { get; }

            internal TestParameters(
                DateTime creationDate,
                TimeSpan idleTimeout,
                TimeSpan absoluteTimeout,
                DateTime today,
                DateTime expectedExpiration)
            {
                CreationDate = creationDate;
                IdleTimeout = idleTimeout;
                AbsoluteTimeout = absoluteTimeout;
                Today = today;
                ExpectedExpiration = expectedExpiration;
            }
        }

        private static RawSession CreateRawSession(DateTime creationDate)
        {
            return new RawSession(new New(SessionId), new SessionData(creationDate));
        }
    }
}
