﻿using System;
using Messerli.Session.Internal;
using Messerli.Session.SessionState;
using Messerli.Session.Storage;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public sealed class RawSessionExtensionTest
    {
        private static readonly DateTime CreationDate = DateTime.UnixEpoch;

        private static readonly SessionId SessionId = new SessionId("foo-bar");

        private static readonly SessionId RenewedSessionId = new SessionId("bar-baz");

        [Fact]
        public void GetIdReturnsIdForNewSession()
        {
            var session = CreateRawSession(new New(SessionId));
            Assert.Equal(SessionId, session.GetId());
        }

        [Fact]
        public void GetIdReturnsIdForExistingSession()
        {
            var session = CreateRawSession(new Existing(SessionId));
            Assert.Equal(SessionId, session.GetId());
        }

        [Fact]
        public void GetIdReturnsNewIdForExistingSessionWithNewId()
        {
            var session = CreateRawSession(new ExistingWithNewId(SessionId, RenewedSessionId));
            Assert.Equal(RenewedSessionId, session.GetId());
        }

        [Fact]
        public void GetIdThrowsForAbandonedSession()
        {
            var session = CreateRawSession(new Abandoned(SessionId));
            Assert.Throws<InvalidOperationException>(() => { session.GetId(); });
        }

        private static RawSession CreateRawSession(ISessionStateVariant state)
        {
            return new RawSession(state, new SessionData(CreationDate));
        }
    }
}
