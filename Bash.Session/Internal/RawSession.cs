using Bash.Session.SessionState;
using Bash.Session.Storage;

namespace Bash.Session.Internal
{
    internal class RawSession
    {
        public ISessionStateVariant State { get; set; }

        public SessionData SessionData { get; set; }

        public bool ReadOnly { get; set; }

        public RawSession(
            ISessionStateVariant state,
            SessionData sessionData)
        {
            State = state;
            SessionData = sessionData;
        }

        public bool IsEmpty() => SessionData.Data.Count == 0;
    }
}