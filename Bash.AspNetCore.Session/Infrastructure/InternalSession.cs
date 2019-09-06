using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public class InternalSession
    {
        public ISessionStateVariant State { get; set; }

        public SessionData SessionData { get; set; }

        public InternalSession(ISessionStateVariant state, SessionData sessionData)
        {
            State = state;
            SessionData = sessionData;
        }
    }
}
