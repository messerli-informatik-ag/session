using System;
using Messerli.Session.Configuration;
using Messerli.Session.Http;
using Microsoft.AspNetCore.Http;

namespace Messerli.Session.AspNetCore.Internal
{
    internal class Response : IResponse
    {
        private readonly HttpContext _httpContext;

        public Response(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public bool AutomaticCacheControl =>
            _httpContext
                .Features
                .Get<PerRequestSessionSettings>()
                .AutomaticCacheControl;

        public void SetCookie(Cookie cookie)
        {
            var maxAge = cookie.Expiration - DateTime.Now;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = cookie.Settings.HttpOnly,
                Secure = MapSecurePreferenceToBool(cookie.Settings.SecurePreference),
                Expires = new DateTimeOffset(cookie.Expiration),
                MaxAge = maxAge > TimeSpan.Zero ? maxAge : TimeSpan.Zero,
            };

            _httpContext.Response.Cookies.Append(
                cookie.Settings.Name.Value,
                cookie.Value,
                cookieOptions);
        }

        public void SetHeader(string name, string value)
        {
            _httpContext.Response.Headers.Append(name, value);
        }

        public bool HasHeader(string name)
        {
            return _httpContext.Response.Headers.ContainsKey(name);
        }

        private bool MapSecurePreferenceToBool(CookieSecurePreference securePreference)
        {
            return securePreference switch
            {
                CookieSecurePreference.Always => true,
                CookieSecurePreference.Never => false,
                CookieSecurePreference.MatchingRequest => _httpContext.Request.IsHttps,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
