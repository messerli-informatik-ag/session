namespace Bash.Session.SessionState
{
    public interface ISessionStateVariant
    {
        T Abandon<T>(IVisitor<T> visitor);
    }
}
