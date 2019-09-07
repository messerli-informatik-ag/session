using Bash.Session.Configuration;

namespace Bash.Session.Infrastructure
{
    public interface IRequest
    {
        string? GetCookie(CookieName name);
    }
}
