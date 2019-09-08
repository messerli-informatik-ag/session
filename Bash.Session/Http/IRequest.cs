using Bash.Session.Configuration;

namespace Bash.Session.Http
{
    public interface IRequest
    {
        string? GetCookie(CookieName name);
    }
}
