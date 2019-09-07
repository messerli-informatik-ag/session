using System.Threading.Tasks;
using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure.Writer
{
    public class OneTimeSessionWriter : IOneTimeSessionWriter
    {
        private readonly InternalSession _session;

        private readonly ISessionStorage _sessionStorage;

        private readonly IDateTimeFactory _dateTimeFactory;

        private readonly TimeoutSettings _timeoutSettings;

        public OneTimeSessionWriter(
            InternalSession session,
            ISessionStorage sessionStorage,
            IDateTimeFactory dateTimeFactory,
            TimeoutSettings timeoutSettings)
        {
            _session = session;
            _sessionStorage = sessionStorage;
            _dateTimeFactory = dateTimeFactory;
            _timeoutSettings = timeoutSettings;
        }

        public async Task VisitNew(New state)
        {
            if (!IsSessionEmpty())
            {
                await WriteSession();
            }
        }

        public async Task VisitExisting(Existing state)
        {
            if (IsSessionEmpty())
            {
                await RemoveSession(state.Id);
            }
            else
            {
                await WriteSession();
            }
        }

        public async Task VisitExistingWithNewId(ExistingWithNewId state)
        {
            await RemoveSession(state.OldId);

            if (!IsSessionEmpty())
            {
                await WriteSession();
            }
        }

        public async Task VisitAbandoned(Abandoned state)
        {
            await RemoveSession(state.Id);
        }

        private async Task RemoveSession(SessionId sessionId)
        {
            await _sessionStorage.RemoveSessionData(sessionId);
        }

        private async Task WriteSession()
        {
            var now = _dateTimeFactory.Now();
            var expiration = now + _timeoutSettings.IdleTimeout;

            await _sessionStorage.WriteSessionData(
                _session.GetId(),
                _session.SessionData,
                expiration);
        }

        private bool IsSessionEmpty()
        {
            return _session.SessionData.Data.Count == 0;
        }
    }
}
