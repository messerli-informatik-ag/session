using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session.Infrastructure
{
    internal static class SessionExtension
    {
        public static SessionId GetId(this InternalSession session)
        {
            return session.State.Abandon(new Visitor());
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
