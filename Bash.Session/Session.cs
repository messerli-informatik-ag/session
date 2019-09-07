using System;
using Bash.Session.Infrastructure;
using Bash.Session.SessionState;

namespace Bash.Session
{
    public class Session : ISession
    {
        private readonly InternalSession _session;
        private readonly Func<IRenewIdVisitor> _createRenewIdVisitor;

        internal Session(
            InternalSession session,
            Func<IRenewIdVisitor> createRenewIdVisitor)
        {
            _session = session;
            _createRenewIdVisitor = createRenewIdVisitor;
        }

        public SessionId Id => _session.GetId();

        public ISessionStateVariant State => _session.State;

        public void RenewId()
        {
            _session.State = State.Abandon(_createRenewIdVisitor());
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
