namespace Bash.Session.SessionState
{
    public class Existing : ISessionStateVariant
    {
        public Existing(SessionId id)
        {
            Id = id;
        }

        public SessionId Id { get; }

        public T Abandon<T>(IVisitor<T> visitor) => visitor.VisitExisting(this);
    }
}
