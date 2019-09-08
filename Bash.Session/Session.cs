using Bash.Session.Infrastructure;
using Bash.Session.SessionState;
using static Bash.Session.Functional;

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
            _session.State = _session.State.Map(
                mapNew: Identity,
                mapExisting: RenewExisting,
                mapExistingWithNewId: Identity,
                mapAbandoned: Identity);
        }

        private ISessionStateVariant RenewExisting(Existing oldState)
        {
            return new ExistingWithNewId(oldState.Id, _sessionIdGenerator.Generate());
        }

        public void Abandon()
        {
            _session.State = new Abandoned(Id);
        }

        public void Set(string key, string value)
        {
            _session.SessionData.Data[key] = value;
        }

        public string? Get(string key)
        {
            _session.SessionData.Data.TryGetValue(key, out var value);
            return value;
        }

        public void Remove(string key) => _session.SessionData.Data.Remove(key);
    }
}
