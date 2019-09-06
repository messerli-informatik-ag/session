namespace Bash.AspNetCore.Session.SessionState
{
    public class New : ISessionStateVariant
    {
        public SessionId Id { get; }

        public void Visit(IVisitor visitor)
        {
            visitor.VisitNew(this);
        }
    }
}
