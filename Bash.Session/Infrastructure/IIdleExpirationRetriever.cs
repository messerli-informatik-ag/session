using System;

namespace Bash.Session.Infrastructure
{
    public interface IIdleExpirationRetriever
    {
        DateTime GetIdleExpiration();
    }
}
