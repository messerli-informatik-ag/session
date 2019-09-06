namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface ISessionIdGenerator
    {
        SessionId Generate();
    }
}
