namespace Bash.Session.Infrastructure
{
    public interface ISessionCreator
    {
        RawSession CreateSession();
    }
}
