using System.Threading.Tasks;
using Bash.Session;
using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure
{
    public class SessionLoader : ISessionLoader
    {
        private readonly ISessionStorage _sessionStorage;
        
        private readonly CookieName _cookieName;

        public SessionLoader(
            ISessionStorage sessionStorage,
            CookieName cookieName)
        {
            _sessionStorage = sessionStorage;
            _cookieName = cookieName;
        }

        public async Task<InternalSession?> LoadFromRequest(IRequest request)
        {
            var sessionIdString = request.GetCookie(_cookieName);

            if (string.IsNullOrWhiteSpace(sessionIdString))
            {
                return null;
            }

            var sessionId = new SessionId(sessionIdString);
            var sessionData = await _sessionStorage.ReadSessionData(sessionId);

            if (sessionData is null)
            {
                return null;
            }

            return new InternalSession(
                new Existing(sessionId),
                sessionData);
        }
    }
}
