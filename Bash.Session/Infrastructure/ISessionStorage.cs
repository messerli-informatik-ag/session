using System;
using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    public interface ISessionStorage
    {
        Task WriteSessionData(SessionId id, SessionData data, DateTime idleExpirationDate);

        Task RemoveSessionData(SessionId id);

        Task<SessionData?> ReadSessionData(SessionId id);
    }
}
