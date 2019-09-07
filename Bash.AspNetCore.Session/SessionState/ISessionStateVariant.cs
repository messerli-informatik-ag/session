namespace Bash.AspNetCore.Session.SessionState
{
    public interface ISessionStateVariant
    {
        T Abandon<T>(IVisitor<T> visitor);
    }
}
