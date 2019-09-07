namespace Bash.Session.SessionState
{
    public class Abandoned : ISessionStateVariant
    {
        public Abandoned(SessionId id)
        {
            Id = id;
        }

        public SessionId Id { get; }

        public T Abandon<T>(IVisitor<T> visitor) => visitor.VisitAbandoned(this);
    }
}
