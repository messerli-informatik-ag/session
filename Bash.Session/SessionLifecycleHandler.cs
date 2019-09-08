using System;
using System.Threading.Tasks;
using Bash.Session.Infrastructure;
using Bash.Session.Infrastructure.Writer;

namespace Bash.Session
{
    internal class SessionLifecycleHandler : ISessionLifecycleHandler
    {
        internal delegate ISession WrapSession(RawSession session);

        private readonly ISessionLoader _sessionLoader;

        private readonly ISessionCreator _sessionCreator;

        private readonly ISessionWriter _sessionWriter;

        private readonly WrapSession _wrapSession;

        private readonly IIdleExpirationRetriever _idleExpirationRetriever;

        private readonly ICookieWriter _cookieWriter;

        private RawSession? _rawSession;

        internal SessionLifecycleHandler(
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

        public ISession Session => _wrapSession(RawSession);

        private RawSession RawSession => 
            _rawSession ?? throw new InvalidOperationException(
                $"{nameof(OnRequest)} must be called before accessing the session");

        public async Task OnRequest(IRequest request)
        {
            var sessionFromRequest = await _sessionLoader.LoadFromRequest(request);
            _rawSession = sessionFromRequest ?? _sessionCreator.CreateSession();
        }

        public async Task OnResponse(IResponse response)
        {
            var idleExpiration = _idleExpirationRetriever.GetIdleExpiration();
            await _sessionWriter.WriteSession(RawSession, idleExpiration);
            _cookieWriter.WriteCookie(response, RawSession, idleExpiration);
        }
    }
}
