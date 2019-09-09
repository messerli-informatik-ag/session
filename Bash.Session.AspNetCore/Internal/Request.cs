using Bash.Session.Configuration;
using Bash.Session.Http;
using Microsoft.AspNetCore.Http;

namespace Bash.Session.AspNetCore.Internal
{
    internal class Request : IRequest
    {
        private readonly HttpContext _httpContext;

        public Request(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public string? GetCookie(CookieName name)
        {
            _httpContext.Request.Cookies.TryGetValue(name.Value, out var value);
            return value;
        }

        public bool HasCookie(CookieName name)
        {
            return _httpContext.Request.Cookies.ContainsKey(name.Value);
        }
    }
}
