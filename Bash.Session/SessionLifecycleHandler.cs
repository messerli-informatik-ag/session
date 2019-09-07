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

        private readonly IIdleExpirationDateRetriever _idleExpirationDateRetriever;

        private RawSession _session;

        public SessionLifecycleHandler(
            ISessionLoader sessionLoader,
            ISessionCreator sessionCreator,
            ISessionWriter sessionWriter,
            WrapSession wrapSession,
            IIdleExpirationDateRetriever idleExpirationDateRetriever)
        {
            _sessionLoader = sessionLoader;
            _sessionCreator = sessionCreator;
            _sessionWriter = sessionWriter;
            _wrapSession = wrapSession;
            _idleExpirationDateRetriever = idleExpirationDateRetriever;
        }

        public ISession Session => _wrapSession(_session);
        
        public async Task OnRequest(IRequest request)
        {
            var sessionFromRequest = await _sessionLoader.LoadFromRequest(request);
            _session = sessionFromRequest ?? _sessionCreator.CreateSession();
        }

        public async Task OnResponse(IResponse response)
        {
            var idleExpirationDate = _idleExpirationDateRetriever.GetIdleExpirationDate();
            await _sessionWriter.WriteSession(_session, idleExpirationDate);
        }
    }
}
