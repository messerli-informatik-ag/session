using System;
using System.Threading.Tasks;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface ISessionStorage
    {
        Task WriteSessionData(SessionId id, SessionData data, DateTime expiration);

        Task RemoveSessionData(SessionId id);

        Task<SessionData?> ReadSessionData(SessionId id);

        Task<bool> SessionDataExists(SessionId id);
    }
}
