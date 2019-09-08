using System;

namespace Bash.Session.Infrastructure
{
    public interface ICookieWriter
    {
        void WriteCookie(IResponse response, RawSession session, DateTime idleExpirationDate);
    }
}
