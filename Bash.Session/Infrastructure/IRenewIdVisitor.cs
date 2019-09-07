using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure
{
    public interface IRenewIdVisitor : IVisitor<ISessionStateVariant>
    {
    }
}
