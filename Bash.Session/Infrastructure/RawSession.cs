using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure
{
    internal class RawSession
    {
        public ISessionStateVariant State { get; set; }

        public SessionData SessionData { get; set; }

        public RawSession(
            ISessionStateVariant state,
            SessionData sessionData)
        {
            State = state;
            SessionData = sessionData;
        }
    }
}
