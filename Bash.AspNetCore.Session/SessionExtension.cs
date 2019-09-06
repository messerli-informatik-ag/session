using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session
{
    public static class SessionExtension
    {
        public static SessionId GetId(this ISession session)
        {
            var visitor = new Visitor();
            session.State.Visit(visitor);
            return visitor.Id;
        }

        private class Visitor : IVisitor
        {
            public SessionId Id { get; private set; }

            public void VisitNew(New state) => Id = state.Id;

            public void VisitExisting(Existing state) => Id = state.Id;

            public void VisitExistingWithNewId(ExistingWithNewId state) => Id = state.NewId;
        }
    }
}
