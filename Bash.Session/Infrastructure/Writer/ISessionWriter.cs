using System;
using System.Threading.Tasks;

namespace Bash.Session.Infrastructure.Writer
{
    public interface ISessionWriter
    {
        Task WriteSession(RawSession session, DateTime idleExpirationDate);
    }
}
