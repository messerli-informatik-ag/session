using System;
using Bash.Session.Configuration;
using Bash.Session.Infrastructure;
using Bash.Session.Infrastructure.Writer;

namespace Bash.Session
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

        public ISessionLifecycleHandler CreateSessionLifeCycleHandler() =>
            new SessionLifecycleHandler(
                CreateSessionLoader(),
                CreateSessionCreator(),
                CreateSessionWriter(),
                CreateSession,
                CreateCookieWriter(),
                CreateIdleExpirationRetriever());

        private IIdleExpirationRetriever CreateIdleExpirationRetriever() =>
            new IdleExpirationRetriever(CreateDateTimeFactory(), _timeoutSettings);

        private ISessionWriter CreateSessionWriter() => new SessionWriter(_sessionStorage);

        private ISessionCreator CreateSessionCreator() =>
            new SessionCreator(_sessionIdGenerator, CreateDateTimeFactory());

        private static IDateTimeFactory CreateDateTimeFactory() => new DateTimeFactory();

        private ISessionLoader CreateSessionLoader() => 
            new SessionLoader(_sessionStorage, _cookieSettings.Name);

        private static ICookieWriter CreateCookieWriter() => throw new NotImplementedException();

        private ISession CreateSession(RawSession rawSession) => 
            new Session(rawSession, _sessionIdGenerator);
    }
}
