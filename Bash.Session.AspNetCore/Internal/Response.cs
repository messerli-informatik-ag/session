using Bash.Session.Http;
using Microsoft.AspNetCore.Http;

namespace Bash.Session.AspNetCore.Internal
{
    internal class Response : IResponse
    {
        private readonly HttpContext _httpContext;

        public Response(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetCookie(Cookie cookie)
        {
            throw new System.NotImplementedException();
        }

        public void SetHeader(string name, string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
