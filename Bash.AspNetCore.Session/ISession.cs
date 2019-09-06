using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session
{
    public interface ISession
    {
        ISessionStateVariant State { get; }

        /// <summary>
        /// Generates a new id for this session.
        /// This does nothing if the session is new.
        /// </summary>
        void RenewId();

        void Set(string key, string value);

        string? Get(string key);
    }
}
