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

        public ISessionStateVariant VisitNew(New state) => state;

        public ISessionStateVariant VisitExisting(Existing state) =>
            new ExistingWithNewId(state.Id, _sessionIdGenerator.Generate());

        public ISessionStateVariant VisitExistingWithNewId(ExistingWithNewId state) => state;

        public ISessionStateVariant VisitAbandoned(Abandoned state) => state;
    }
}
