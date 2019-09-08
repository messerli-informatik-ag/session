using System;

namespace Bash.Session.Infrastructure
{
    internal interface ICookieWriter
    {
        void WriteCookie(IResponse response, RawSession session, DateTime idleExpirationDate);
    }
}
