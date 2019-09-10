using System;

namespace Bash.Session
{
    public interface ISession
    {
        /// <exception cref="InvalidOperationException">When trying to retrieve the id of an abandoned session</exception>
        SessionId Id { get; }

        /// <summary>
        /// Generates a new id for this session.
        /// This does nothing if the session is new.
        /// </summary>
        /// <exception cref="InvalidOperationException">When trying to renew the id of an abandoned session</exception>
        /// <exception cref="InvalidOperationException">When the session is in read-only mode.</exception>
        void RenewId();

        /// <summary>
        /// Abandons this session. It will be removed from the storage and
        /// the session id cookie will be removed from the client.
        /// </summary>
        /// <exception cref="InvalidOperationException">When the session is in read-only mode.</exception>
        void Abandon();

        /// <exception cref="InvalidOperationException">When the session is in read-only mode.</exception>
        void Set(string key, byte[] value);

        byte[]? Get(string key);

        /// <exception cref="InvalidOperationException">When the session is in read-only mode.</exception>
        void Remove(string key);
    }
}
