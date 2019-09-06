using System;
using System.Threading.Tasks;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface ISessionStorage
    {
        Task WriteSessionData(SessionId id, SessionData data);

        Task RemoveSessionData(SessionId id);

        /// <summary>
        /// Moves the session data to a new session.
        /// This operation must be implemented atomically on the data storage.
        /// </summary>
        /// <exception cref="ArgumentException">When the session with the id <paramref name="oldId" /> does not exist</exception>
        Task MoveSessionData(SessionId oldId, SessionId newId);

        Task<SessionData?> ReadSessionData(SessionId id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> SessionDataExists(SessionId id);
    }
}
