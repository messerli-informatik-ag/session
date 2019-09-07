using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface IRenewIdVisitor : IVisitor<ISessionStateVariant>
    {
    }
}
