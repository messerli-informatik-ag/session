using Messerli.Session.Configuration;

namespace Messerli.Session.Http
{
    public interface IRequest
    {
        string? GetCookie(CookieName name);

        bool HasCookie(CookieName name);
    }
}
