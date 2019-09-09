using System;
using Bash.Session.Http;

namespace Bash.Session.Internal
{
    internal interface ICookieWriter
    {
        void WriteCookie(IRequest request, IResponse response, RawSession session, DateTime idleExpirationDate);
    }
}
