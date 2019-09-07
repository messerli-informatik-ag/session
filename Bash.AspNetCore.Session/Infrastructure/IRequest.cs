namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface IRequest
    {
        string? GetCookie(CookieName name);
    }
}
