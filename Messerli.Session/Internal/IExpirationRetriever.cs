using System;

namespace Messerli.Session.Internal
{
    internal interface IExpirationRetriever
    {
        DateTime GetExpiration(RawSession session);
    }
}
