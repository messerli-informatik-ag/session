using System;
using Bash.AspNetCore.Session.Infrastructure;
using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session
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

        public ISessionStateVariant State => _session.State;

        public void RenewId()
        {
            var visitor = _createRenewIdVisitor();
            State.Visit(visitor);
            _session.State = visitor.NewState;
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
