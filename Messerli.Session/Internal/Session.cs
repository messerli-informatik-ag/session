using System;
using Messerli.Session.SessionState;
using static Messerli.Session.Utility.Functional;

namespace Messerli.Session.Internal
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

        public DateTime CreationDate => _session.SessionData.CreationDate;

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
            ValidateKey(key);
            _session.SessionData.Data[key] = value;
        }

        public byte[]? Get(string key)
        {
            ValidateKey(key);
            _session.SessionData.Data.TryGetValue(key, out var value);
            return value;
        }

        public void Remove(string key)
        {
            ValidateKey(key);
            AssertSessionIsWritable();
            _session.SessionData.Data.Remove(key);
        }

        private void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key should not consist of whitespace only", nameof(key));
            }
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
