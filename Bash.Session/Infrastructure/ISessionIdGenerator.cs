namespace Bash.Session.Infrastructure
{
    public interface ISessionIdGenerator
    {
        SessionId Generate();
    }
}
