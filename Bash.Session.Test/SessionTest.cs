using System;
using Bash.Session.Internal;
using Bash.Session.SessionState;
using Bash.Session.Storage;
using Moq;
using Xunit;

namespace Bash.Session.Test
{
    public class SessionTest
    {
        private static readonly SessionId SessionId = new SessionId("foo");

        private static readonly SessionId RenewedSessionId = new SessionId("renewed");

        private static readonly DateTime Created = DateTime.UnixEpoch;

        [Fact]
        public void SetThrowsIfRawSessionIsReadonly()
        {
            var session = CreateReadOnlySession();
            Assert.Throws<InvalidOperationException>(() => session.SetString("foo", "bar"));
        }

        [Fact]
        public void RenewIdThrowsIfRawSessionIsReadonly()
        {
            var session = CreateReadOnlySession();
            Assert.Throws<InvalidOperationException>(() => session.RenewId());
        }

        [Fact]
        public void AbandonThrowsIfRawSessionIsReadonly()
        {
            var session = CreateReadOnlySession();
            Assert.Throws<InvalidOperationException>(() => session.Abandon());
        }

        [Fact]
        public void RemoveThrowsIfRawSessionIsReadonly()
        {
            var session = CreateReadOnlySession();
            Assert.Throws<InvalidOperationException>(() => session.Remove("Foo"));
        }

        [Fact]
        public void RenewIdDoesNothingForNewSession()
        {
            TestRenewsSessionId(new New(SessionId), new New(SessionId));
        }

        [Fact]
        public void RenewIdDoesNothingForExistingSessionWithNewId()
        {
            TestRenewsSessionId(new ExistingWithNewId(SessionId, RenewedSessionId), new ExistingWithNewId(SessionId, RenewedSessionId));
        }

        [Fact]
        public void RenewIdDoesNothingForAbandonedSession()
        {
            TestRenewsSessionId(new Abandoned(SessionId), new Abandoned(SessionId));
        }

        [Fact]
        public void RenewIdRenewsIdForExistingSession()
        {
            TestRenewsSessionId(new Existing(SessionId), new ExistingWithNewId(SessionId, RenewedSessionId));
        }

        private static void TestRenewsSessionId(ISessionStateVariant state, ISessionStateVariant expectedState)
        {
            var rawSession = new RawSession(state, new SessionData(Created));
            var session = CreateSession(rawSession);
            session.RenewId();
            Assert.Equal(rawSession.State, expectedState);
        }

        private static ISession CreateReadOnlySession()
        {
            return CreateSession(CreateReadOnlyRawSession());
        }

        private static RawSession CreateReadOnlyRawSession()
        {
            var rawSession = CreateRawSession();
            rawSession.ReadOnly = true;
            return rawSession;
        }

        private static RawSession CreateRawSession()
        {
            return new RawSession(new New(SessionId), new SessionData(Created));
        }

        private static ISession CreateSession(RawSession rawSession)
        {
            return new Session(rawSession, MockSessionIdGenerator());
        }

        private static ISessionIdGenerator MockSessionIdGenerator()
        {
            var sessionIdGenerator = new Mock<ISessionIdGenerator>();
            sessionIdGenerator.Setup(g => g.Generate())
                .Returns(RenewedSessionId);
            return sessionIdGenerator.Object;
        }
    }
}
