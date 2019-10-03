using System;
using Messerli.Session.Http;

namespace Messerli.Session.Internal
{
    internal interface ICookieWriter
    {
        void WriteCookie(IRequest request, IResponse response, RawSession session, DateTime expirationDate);
    }
}
