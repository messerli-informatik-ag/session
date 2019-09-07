using System.Threading.Tasks;
using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure.Writer
{
    public class SessionWriter : ISessionWriter
    {
        private readonly ISessionStorage _sessionStorage;

        private readonly IDateTimeFactory _dateTimeFactory;

        private readonly TimeoutSettings _timeoutSettings;

        public SessionWriter(
            ISessionStorage sessionStorage,
            IDateTimeFactory dateTimeFactory,
            TimeoutSettings timeoutSettings)
        {
            _sessionStorage = sessionStorage;
            _dateTimeFactory = dateTimeFactory;
            _timeoutSettings = timeoutSettings;
        }

        public async Task WriteSession(RawSession session)
        {
            await session.State.Map(
                mapNew: _ => WriteNew(session),
                mapExisting: state => WriteExisting(state, session),
                mapExistingWithNewId: state => WriteExistingWithNewId(state, session),
                mapAbandoned: state => RemoveSession(state.Id));
        }

        private async Task WriteNew(RawSession session)
        {
            if (!IsSessionEmpty(session))
            {
                await WriteSessionData(session);
            }
        }

        private async Task WriteExisting(Existing state, RawSession session)
        {
            if (IsSessionEmpty(session))
            {
                await RemoveSession(state.Id);
            }
            else
            {
                await WriteSessionData(session);
            }
        }

        private async Task WriteExistingWithNewId(ExistingWithNewId state, RawSession session)
        {
            await RemoveSession(state.OldId);

            if (!IsSessionEmpty(session))
            {
                await WriteSessionData(session);
            }
        }

        private async Task RemoveSession(SessionId sessionId)
        {
            await _sessionStorage.RemoveSessionData(sessionId);
        }

        private async Task WriteSessionData(RawSession session)
        {
            var now = _dateTimeFactory.Now();
            var expiration = now + _timeoutSettings.IdleTimeout;

            await _sessionStorage.WriteSessionData(
                session.GetId(),
                session.SessionData,
                expiration);
        }

        private static bool IsSessionEmpty(RawSession session)
        {
            return session.SessionData.Data.Count == 0;
        }
    }
}
