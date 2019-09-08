namespace Bash.Session.Infrastructure
{
    internal interface ISessionCreator
    {
        RawSession CreateSession();
    }
}
