using System.Threading.Tasks;
using Bash.Session.Infrastructure;
using Bash.Session.Infrastructure.Writer;

namespace Bash.Session
{
    public class SessionLifecycleHandler : ISessionLifecycleHandler
    {
        public delegate ISession WrapSession(RawSession session);
        
        private readonly ISessionLoader _sessionLoader;

        private readonly ISessionCreator _sessionCreator;

        private readonly ISessionWriter _sessionWriter;

        private readonly WrapSession _wrapSession;

        private readonly IIdleExpirationRetriever _idleExpirationRetriever;

        private readonly ICookieWriter _cookieWriter;

        private RawSession _session;

        public SessionLifecycleHandler(
            ISessionLoader sessionLoader,
            ISessionCreator sessionCreator,
            ISessionWriter sessionWriter,
            WrapSession wrapSession,
            ICookieWriter cookieWriter,
            IIdleExpirationRetriever idleExpirationRetriever)
        {
            _sessionLoader = sessionLoader;
            _sessionCreator = sessionCreator;
            _sessionWriter = sessionWriter;
            _wrapSession = wrapSession;
            _cookieWriter = cookieWriter;
            _idleExpirationRetriever = idleExpirationRetriever;
        }

        public ISession Session => _wrapSession(_session);
        
        public async Task OnRequest(IRequest request)
        {
            var sessionFromRequest = await _sessionLoader.LoadFromRequest(request);
            _session = sessionFromRequest ?? _sessionCreator.CreateSession();
        }

        public async Task OnResponse(IResponse response)
        {
            var idleExpiration = _idleExpirationRetriever.GetIdleExpiration();
            await _sessionWriter.WriteSession(_session, idleExpiration);
            _cookieWriter.WriteCookie(response, _session, idleExpiration);
        }
    }
}
