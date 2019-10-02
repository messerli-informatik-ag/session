using System;
using System.Threading.Tasks;
using Messerli.Session.Http;
using Messerli.Session.Internal.Writer;

namespace Messerli.Session.Internal
{
    internal class SessionLifecycleHandler : ISessionLifecycleHandler
    {
        private readonly ISessionLoader _sessionLoader;

        private readonly ISessionCreator _sessionCreator;

        private readonly ISessionWriter _sessionWriter;

        private readonly WrapSession _wrapSession;

        private readonly IExpirationRetriever _expirationRetriever;

        private readonly ICookieWriter _cookieWriter;

        private RawSession? _rawSession;

        internal SessionLifecycleHandler(
            ISessionLoader sessionLoader,
            ISessionCreator sessionCreator,
            ISessionWriter sessionWriter,
            WrapSession wrapSession,
            ICookieWriter cookieWriter,
            IExpirationRetriever expirationRetriever)
        {
            _sessionLoader = sessionLoader;
            _sessionCreator = sessionCreator;
            _sessionWriter = sessionWriter;
            _wrapSession = wrapSession;
            _cookieWriter = cookieWriter;
            _expirationRetriever = expirationRetriever;
        }

        internal delegate ISession WrapSession(RawSession session);

        public ISession Session => _wrapSession(RawSession);

        private RawSession RawSession =>
            _rawSession ?? throw new InvalidOperationException(
                $"{nameof(OnRequest)} must be called before accessing the session");

        public async Task OnRequest(IRequest request)
        {
            var sessionFromRequest = await _sessionLoader.LoadFromRequest(request);
            _rawSession = sessionFromRequest ?? _sessionCreator.CreateSession();
        }

        public async Task OnResponse(IRequest request, IResponse response)
        {
            RawSession.ReadOnly = true;
            var idleExpiration = _expirationRetriever.GetExpiration(RawSession);
            await _sessionWriter.WriteSession(RawSession, idleExpiration);
            _cookieWriter.WriteCookie(request, response, RawSession, idleExpiration);
        }
    }
}
