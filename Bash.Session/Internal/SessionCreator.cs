using Bash.Session.SessionState;
using Bash.Session.Storage;

namespace Bash.Session.Internal
{
    internal class SessionCreator : ISessionCreator
    {
        private readonly ISessionIdGenerator _sessionIdGenerator;

        private readonly IDateTimeFactory _dateTimeFactory;

        public SessionCreator(
            ISessionIdGenerator sessionIdGenerator,
            IDateTimeFactory dateTimeFactory)
        {
            _sessionIdGenerator = sessionIdGenerator;
            _dateTimeFactory = dateTimeFactory;
        }

        public RawSession CreateSession()
        {
            var sessionId = _sessionIdGenerator.Generate();
            var now = _dateTimeFactory.Now();
            return new RawSession(
                new New(sessionId),
                new SessionData(now));
        }
    }
}
