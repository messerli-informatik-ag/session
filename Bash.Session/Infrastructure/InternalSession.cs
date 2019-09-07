using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure
{
    public class InternalSession
    {
        public ISessionStateVariant State { get; set; }

        public SessionData SessionData { get; set; }

        public InternalSession(
            ISessionStateVariant state,
            SessionData sessionData)
        {
            State = state;
            SessionData = sessionData;
        }
    }
}
