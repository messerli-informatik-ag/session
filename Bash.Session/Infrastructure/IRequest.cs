using Bash.Session;

namespace Bash.Session.Infrastructure
{
    public interface IRequest
    {
        string? GetCookie(CookieName name);
    }
}
