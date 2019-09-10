using System;
using Bash.Session.Internal;
using Bash.Session.SessionState;
using static Bash.Session.Utility.Functional;

namespace Bash.Session
{
    internal class Session : ISession
    {
        private readonly RawSession _session;
        private readonly ISessionIdGenerator _sessionIdGenerator;

        internal Session(
            RawSession session,
            ISessionIdGenerator sessionIdGenerator)
        {
            _session = session;
            _sessionIdGenerator = sessionIdGenerator;
        }

        public SessionId Id => _session.GetId();

        public void RenewId()
        {
            AssertSessionIsWritable();
            _session.State = _session.State.Map(
                mapNew: Identity,
                mapExisting: RenewExisting,
                mapExistingWithNewId: Identity,
                mapAbandoned: _ => throw new InvalidOperationException("Trying to renew the id of an abandoned session"));
        }

        private ISessionStateVariant RenewExisting(Existing oldState)
        {
            return new ExistingWithNewId(oldState.Id, _sessionIdGenerator.Generate());
        }

        public void Abandon()
        {
            AssertSessionIsWritable();
            _session.State = _session.State.Map(
                mapNew: state => new Abandoned(null),
                mapExisting: state => new Abandoned(state.Id),
                mapExistingWithNewId: state => new Abandoned(state.OldId),
                mapAbandoned: Identity);
        }

        public void Set(string key, byte[] value)
        {
            AssertSessionIsWritable();
            _session.SessionData.Data[key] = value;
        }

        public byte[]? Get(string key)
        {
            _session.SessionData.Data.TryGetValue(key, out var value);
            return value;
        }

        public void Remove(string key)
        {
            AssertSessionIsWritable();
            _session.SessionData.Data.Remove(key);
        }

        private void AssertSessionIsWritable()
        {
            if (_session.ReadOnly)
            {
                throw new InvalidOperationException("Read-only session can't be modified");
            }
        }
    }
}
