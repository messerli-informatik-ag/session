namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface IRequest
    {
        string? GetCookie(string name);
    }
}
