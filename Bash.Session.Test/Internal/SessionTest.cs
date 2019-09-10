using System;
using Bash.Session.Internal;
using Bash.Session.SessionState;
using Bash.Session.Storage;
using Moq;
using Xunit;

namespace Bash.Session.Test.Internal
{
    public class SessionTest
    {
        private static readonly SessionId SessionId = new SessionId("foo");

        private static readonly SessionId RenewedSessionId = new SessionId("renewed");

        private static readonly DateTime CreationDate = DateTime.UnixEpoch;

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
        public void RenewIdThrowsForAbandonedSession()
        {
            var session = CreateSession(CreateRawSession());
            session.Abandon();
            Assert.Throws<InvalidOperationException>(() => session.RenewId());
        }

        [Fact]
        public void RenewIdRenewsIdForExistingSession()
        {
            TestRenewsSessionId(new Existing(SessionId), new ExistingWithNewId(SessionId, RenewedSessionId));
        }

        [Fact]
        public void AbandonsNewSession()
        {
            TestAbandonsSession(new New(SessionId), new Abandoned(null));
        }

        [Fact]
        public void AbandonsExistingSession()
        {
            TestAbandonsSession(new Existing(SessionId), new Abandoned(SessionId));
        }

        [Fact]
        public void AbandonsExistingSessionWithNewId()
        {
            TestAbandonsSession(new ExistingWithNewId(SessionId, RenewedSessionId), new Abandoned(SessionId));
        }

        [Fact]
        public void DoesNothingWhenAbandoningAbandonedSession()
        {
            TestAbandonsSession(new Abandoned(SessionId), new Abandoned(SessionId));
        }

        [Theory]
        [MemberData(nameof(InvalidSessionDataKeys))]
        public void SetThrowsForInvalidKeys(string invalidKey)
        {
            var session = CreateSession(CreateRawSession());
            Assert.Throws<ArgumentException>(() =>
            {
                session.Set(invalidKey, new byte[] { 0x1 });
            });
        }

        [Theory]
        [MemberData(nameof(InvalidSessionDataKeys))]
        public void GetThrowsForInvalidKeys(string invalidKey)
        {
            var session = CreateSession(CreateRawSession());
            Assert.Throws<ArgumentException>(() =>
            {
                session.Get(invalidKey);
            });
        }

        [Theory]
        [MemberData(nameof(InvalidSessionDataKeys))]
        public void RemoveThrowsForInvalidKeys(string invalidKey)
        {
            var session = CreateSession(CreateRawSession());
            Assert.Throws<ArgumentException>(() =>
            {
                session.Remove(invalidKey);
            });
        }

        [Fact]
        public void ValueCanBeWrittenAndRead()
        {
            const string key = "foo";
            var value = new byte[] { 0x1, 0x2, 0x3 };
            var session = CreateSession(CreateRawSession());
            session.Set(key, value);
            Assert.Equal(value, session.Get(key));
        }

        [Fact]
        public void ValueCanBeRemoved()
        {
            const string key = "foo";
            var value = new byte[] { 0x1, 0x2, 0x3 };
            var session = CreateSession(CreateRawSession());
            session.Set(key, value);
            session.Remove(key);
            Assert.Null(session.Get(key));
        }

        [Fact]
        public void IdCanBeAccessed()
        {
            var session = CreateSession(CreateRawSession());
            Assert.Equal(SessionId, session.Id);
        }

        [Fact]
        public void CreatedDateCanBeAccessed()
        {
            var session = CreateSession(CreateRawSession());
            Assert.Equal(CreationDate, session.CreationDate);
        }

        public static TheoryData<string> InvalidSessionDataKeys()
        {
            return new TheoryData<string>
            {
                "",
                " ",
                "\t",
                "\n",
                "\r",
                "\r\n",
            };
        }

        private static void TestRenewsSessionId(ISessionStateVariant state, ISessionStateVariant expectedState)
        {
            var rawSession = new RawSession(state, new SessionData(CreationDate));
            var session = CreateSession(rawSession);
            session.RenewId();
            Assert.Equal(rawSession.State, expectedState);
        }

        private static void TestAbandonsSession(ISessionStateVariant state, ISessionStateVariant expectedState)
        {
            var rawSession = new RawSession(state, new SessionData(CreationDate));
            var session = CreateSession(rawSession);
            session.Abandon();
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
            return new RawSession(new New(SessionId), new SessionData(CreationDate));
        }

        private static ISession CreateSession(RawSession rawSession)
        {
            return new Internal.Session(rawSession, MockSessionIdGenerator());
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
