using System;
using System.Threading.Tasks;

namespace Messerli.Session.Storage
{
    public interface ISessionStorage
    {
        Task WriteSessionData(SessionId id, SessionData data, DateTime idleExpirationDate);

        Task RemoveSessionData(SessionId id);

        Task<SessionData?> ReadSessionData(SessionId id);
    }
}
