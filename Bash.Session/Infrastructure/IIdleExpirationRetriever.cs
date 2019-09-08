using System;

namespace Bash.Session.Infrastructure
{
    internal interface IIdleExpirationRetriever
    {
        DateTime GetIdleExpiration();
    }
}
