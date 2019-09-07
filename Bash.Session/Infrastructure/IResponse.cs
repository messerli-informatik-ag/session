using System;
using Bash.Session;

namespace Bash.Session.Infrastructure
{
    public interface IResponse
    {
        void SetCookie(CookieName name, string value, DateTime expiration);
    }
}
