using Bash.Session.SessionState;

namespace Bash.Session
{
    public interface ISession
    {
        SessionId Id { get; }
        
        ISessionStateVariant State { get; }

        /// <summary>
        /// Generates a new id for this session.
        /// This does nothing if the session is new.
        /// </summary>
        void RenewId();

        /// <summary>
        /// Abandons this session. It will be removed from the storage and
        /// the session id cookie will be removed from the client.
        /// </summary>
        void Abandon();

        void Set(string key, string value);

        string? Get(string key);
    }
}
