using System;
using System.Threading.Tasks;
using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure.Writer
{
    internal class SessionWriter : ISessionWriter
    {
        private readonly ISessionStorage _sessionStorage;

        public SessionWriter(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task WriteSession(RawSession session, DateTime idleExpirationDate)
        {
            await session.State.Map(
                mapNew: _ => WriteNew(session, idleExpirationDate),
                mapExisting: state => WriteExisting(state, session, idleExpirationDate),
                mapExistingWithNewId: state => WriteExistingWithNewId(state, session, idleExpirationDate),
                mapAbandoned: state => RemoveSession(state.Id));
        }

        private async Task WriteNew(RawSession session, DateTime idleExpirationDate)
        {
            if (!IsSessionEmpty(session))
            {
                await WriteSessionData(session, idleExpirationDate);
            }
        }

        private async Task WriteExisting(Existing state, RawSession session, DateTime idleExpirationDate)
        {
            if (IsSessionEmpty(session))
            {
                await RemoveSession(state.Id);
            }
            else
            {
                await WriteSessionData(session, idleExpirationDate);
            }
        }

        private async Task WriteExistingWithNewId(ExistingWithNewId state, RawSession session, DateTime idleExpirationDate)
        {
            await RemoveSession(state.OldId);

            if (!IsSessionEmpty(session))
            {
                await WriteSessionData(session, idleExpirationDate);
            }
        }

        private async Task RemoveSession(SessionId sessionId)
        {
            await _sessionStorage.RemoveSessionData(sessionId);
        }

        private async Task WriteSessionData(RawSession session, DateTime idleExpirationDate)
        {
            await _sessionStorage.WriteSessionData(
                session.GetId(),
                session.SessionData,
                idleExpirationDate);
        }

        private static bool IsSessionEmpty(RawSession session)
        {
            return session.SessionData.Data.Count == 0;
        }
    }
}
