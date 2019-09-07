namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface ISessionCreator
    {
        InternalSession CreateSession();
    }
}
