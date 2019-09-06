namespace Bash.AspNetCore.Session.SessionState
{
    public interface ISessionStateVariant
    {
        void Visit(IVisitor visitor);
    }
}
