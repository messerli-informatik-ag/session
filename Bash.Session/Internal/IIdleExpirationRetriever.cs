using System;

namespace Bash.Session.Internal
{
    internal interface IIdleExpirationRetriever
    {
        DateTime GetIdleExpiration();
    }
}
