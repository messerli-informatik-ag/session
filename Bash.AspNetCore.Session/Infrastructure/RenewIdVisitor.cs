using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public class RenewIdVisitor : IRenewIdVisitor
    {
        private readonly ISessionIdGenerator _sessionIdGenerator;

        public RenewIdVisitor(ISessionIdGenerator sessionIdGenerator)
        {
            _sessionIdGenerator = sessionIdGenerator;
        }

        public ISessionStateVariant NewState { get; private set; }

        public void VisitNew(New state) => NewState = state;

        public void VisitExisting(Existing state) =>
            NewState = new ExistingWithNewId(state.Id, _sessionIdGenerator.Generate());

        public void VisitExistingWithNewId(ExistingWithNewId state) => NewState = state;
    }
}
