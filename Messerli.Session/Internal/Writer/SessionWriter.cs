using System;
using System.Threading.Tasks;
using Messerli.Session.SessionState;
using Messerli.Session.Storage;

namespace Messerli.Session.Internal.Writer
{
    internal class SessionWriter : ISessionWriter
    {
        private readonly ISessionStorage _sessionStorage;

        public SessionWriter(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task WriteSession(RawSession session, DateTime expirationDate)
        {
            await session.State.Map(
                mapNew: _ => WriteNew(session, expirationDate),
                mapExisting: state => WriteExisting(state, session, expirationDate),
                mapExistingWithNewId: state => WriteExistingWithNewId(state, session, expirationDate),
                mapAbandoned: WriteAbandonedSession);
        }

        private async Task WriteNew(RawSession session, DateTime idleExpirationDate)
        {
            if (!session.IsEmpty())
            {
                await WriteSessionData(session, idleExpirationDate);
            }
        }

        private async Task WriteExisting(Existing state, RawSession session, DateTime expirationDate)
        {
            if (session.IsEmpty())
            {
                await RemoveSession(state.Id);
            }
            else
            {
                await WriteSessionData(session, expirationDate);
            }
        }

        private async Task WriteExistingWithNewId(ExistingWithNewId state, RawSession session, DateTime expirationDate)
        {
            await RemoveSession(state.OldId);

            if (!session.IsEmpty())
            {
                await WriteSessionData(session, expirationDate);
            }
        }

        private async Task WriteAbandonedSession(Abandoned state)
        {
            if (state.Id is { } id)
            {
                await RemoveSession(id);
            }
        }

        private async Task RemoveSession(SessionId sessionId)
        {
            await _sessionStorage.RemoveSessionData(sessionId);
        }

        private async Task WriteSessionData(RawSession session, DateTime expirationDate)
        {
            await _sessionStorage.WriteSessionData(
                session.GetId(),
                session.SessionData,
                expirationDate);
        }
    }
}
