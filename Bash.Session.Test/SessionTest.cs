using System;
using Bash.Session.Internal;
using Bash.Session.SessionState;
using Bash.Session.Storage;
using Xunit;

namespace Bash.Session.Test
{
    public class SessionTest
    {
        private static readonly SessionId SessionId = new SessionId("foo");

        private static readonly DateTime Created = DateTime.UnixEpoch;

        [Fact]
        public void SetThrowsIfRawSessionIsReadonly()
        {
            var session = CreateReadOnlySession();
            Assert.Throws<InvalidOperationException>(() => session.Set("foo", "bar"));
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
            return new Session(rawSession, new SessionIdGenerator());
        }
    }
}
