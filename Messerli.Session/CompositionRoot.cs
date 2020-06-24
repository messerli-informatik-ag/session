using Messerli.Session.Configuration;
using Messerli.Session.Internal;
using Messerli.Session.Internal.Writer;
using Messerli.Session.Storage;

namespace Messerli.Session
{
    public sealed class CompositionRoot
    {
        private readonly ISessionStorage _sessionStorage;

        private readonly CookieSettings _cookieSettings;

        private readonly TimeoutSettings _timeoutSettings;

        private readonly ISessionIdGenerator _sessionIdGenerator;

        internal CompositionRoot(
            ISessionStorage sessionStorage,
            CookieSettings cookieSettings,
            TimeoutSettings timeoutSettings,
            ISessionIdGenerator sessionIdGenerator)
        {
            _sessionStorage = sessionStorage;
            _cookieSettings = cookieSettings;
            _timeoutSettings = timeoutSettings;
            _sessionIdGenerator = sessionIdGenerator;
        }

        public ISessionLifecycleHandler CreateSessionLifeCycleHandler()
            => new SessionLifecycleHandler(
                   CreateSessionLoader(),
                   CreateSessionCreator(),
                   CreateSessionWriter(),
                   CreateSession,
                   CreateCookieWriter(),
                   CreateExpirationRetriever());

        private IExpirationRetriever CreateExpirationRetriever()
            => new ExpirationRetriever(CreateDateTimeFactory(), _timeoutSettings);

        private ISessionWriter CreateSessionWriter()
            => new SessionWriter(_sessionStorage);

        private ISessionCreator CreateSessionCreator()
            => new SessionCreator(_sessionIdGenerator, CreateDateTimeFactory());

        private static IDateTimeFactory CreateDateTimeFactory()
            => new DateTimeFactory();

        private ISessionLoader CreateSessionLoader()
            => new SessionLoader(_sessionStorage, _cookieSettings.Name);

        private ICookieWriter CreateCookieWriter()
            => new CookieWriter(_cookieSettings, CreateCacheControlHeaderWriter());

        private static ICacheControlHeaderWriter CreateCacheControlHeaderWriter()
            => new CacheControlHeaderWriter();

        private ISession CreateSession(RawSession rawSession)
            => new Internal.Session(rawSession, _sessionIdGenerator);
    }
}
