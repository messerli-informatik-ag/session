namespace Bash.AspNetCore.Session.SessionState
{
    public class New : ISessionStateVariant
    {
        public New(SessionId id)
        {
            Id = id;
        }

        public SessionId Id { get; }

        public T Abandon<T>(IVisitor<T> visitor) => visitor.VisitNew(this);
    }
}
