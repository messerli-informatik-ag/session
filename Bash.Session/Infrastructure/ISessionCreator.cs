namespace Bash.Session.Infrastructure
{
    public interface ISessionCreator
    {
        InternalSession CreateSession();
    }
}
