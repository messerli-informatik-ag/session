using System;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface IResponse
    {
        void SetCookie(CookieName name, string value, DateTime expiration);
    }
}
