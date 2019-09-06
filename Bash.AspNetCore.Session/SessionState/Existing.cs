namespace Bash.AspNetCore.Session.SessionState
{
    public class Existing : ISessionStateVariant
    {
        public SessionId Id { get; }

        public void Visit(IVisitor visitor)
        {
            visitor.VisitExisting(this);
        }
    }
}
