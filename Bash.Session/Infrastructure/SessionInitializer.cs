using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    public class SessionInitializer : ISessionInitializer
    {
        private readonly ISessionLoader _sessionLoader;

        private readonly ISessionCreator _sessionCreator;

        public SessionInitializer(
            ISessionLoader sessionLoader,
            ISessionCreator sessionCreator)
        {
            _sessionLoader = sessionLoader;
            _sessionCreator = sessionCreator;
        }

        public async Task<InternalSession> InitializeSession(IRequest request)
        {
            return await _sessionLoader.LoadFromRequest(request)
                   ?? _sessionCreator.CreateSession();
        }
    }
}
