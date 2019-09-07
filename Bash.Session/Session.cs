using Bash.Session.Infrastructure;
using Bash.Session.SessionState;

namespace Bash.Session
{
    public class Session : ISession
    {
        private readonly InternalSession _session;
        private readonly ISessionIdGenerator _sessionIdGenerator;

        internal Session(
            InternalSession session,
            ISessionIdGenerator sessionIdGenerator)
        {
            _session = session;
            _sessionIdGenerator = sessionIdGenerator;
        }

        public SessionId Id => _session.GetId();

        public ISessionStateVariant State => _session.State;

        public void RenewId()
        {
            _session.State = State.Map(
                mapNew: state => state,
                mapExisting: RenewExisting,
                mapExistingWithNewId: state => state,
                mapAbandoned: state => state);
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
    }
}
