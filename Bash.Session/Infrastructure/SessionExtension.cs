using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure
{
    internal static class SessionExtension
    {
        public static SessionId GetId(this InternalSession session)
        {
            return session.State.Accept(new Visitor());
        }

        private class Visitor : IVisitor<SessionId>
        {
            public SessionId VisitNew(New state) => state.Id;

            public SessionId VisitExisting(Existing state) => state.Id;

            public SessionId VisitExistingWithNewId(ExistingWithNewId state) => state.NewId;

            public SessionId VisitAbandoned(Abandoned state) => state.Id;
        }
    }
}
