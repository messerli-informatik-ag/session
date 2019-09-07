using System;

namespace Bash.Session.Infrastructure
{
    public interface IIdleExpirationDateRetriever
    {
        DateTime GetIdleExpirationDate();
    }
}
