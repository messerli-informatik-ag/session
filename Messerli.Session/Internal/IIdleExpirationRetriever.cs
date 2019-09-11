using System;

namespace Messerli.Session.Internal
{
    internal interface IIdleExpirationRetriever
    {
        DateTime GetIdleExpiration();
    }
}
