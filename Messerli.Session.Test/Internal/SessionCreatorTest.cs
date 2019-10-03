using System;
using Messerli.Session.Internal;
using Messerli.Session.SessionState;
using Messerli.Session.Storage;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public class SessionCreatorTest
    {
        [Fact]
        public void CreatesNewSession()
        {
            var sessionId = new SessionId("foo-bar-baz");
            var sessionState = new New(sessionId);
            var creationDate = DateTime.UnixEpoch;
            var expectedSession = new RawSession(sessionState, new SessionData(creationDate));

            var sessionIdGenerator = new Mock<ISessionIdGenerator>();
            sessionIdGenerator.Setup(g => g.Generate())
                .Returns(sessionId);
            var dateTimeFactory = new Mock<IDateTimeFactory>();
            dateTimeFactory.Setup(f => f.Now())
                .Returns(creationDate);
            var sessionCreator = new SessionCreator(sessionIdGenerator.Object, dateTimeFactory.Object);

            Assert.Equal(expectedSession, sessionCreator.CreateSession());
        }
    }
}
