namespace Bash.AspNetCore.Session.SessionState
{
    public class ExistingWithNewId : ISessionStateVariant
    {
        public SessionId OldId { get; }
        
        public SessionId NewId { get; }

        public ExistingWithNewId(SessionId oldId, SessionId newId)
        {
            OldId = oldId;
            NewId = newId;
        }

        public T Abandon<T>(IVisitor<T> visitor) => visitor.VisitExistingWithNewId(this);
    }
}
