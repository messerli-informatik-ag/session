using System;
using Bash.Session.Configuration;
using Bash.Session.Infrastructure;

namespace Bash.Session
{
    public sealed class CompositionRootBuilder
    {
        private readonly ISessionStorage? _sessionStorage;

        private readonly CookieSettings? _cookieSettings;

        private readonly TimeoutSettings? _timeoutSettings;

        private readonly ISessionIdGenerator? _sessionIdGenerator;

        public CompositionRootBuilder()
        {
        }

        private CompositionRootBuilder(
            ISessionStorage? sessionStorage,
            CookieSettings? cookieSettings,
            TimeoutSettings? timeoutSettings,
            ISessionIdGenerator? sessionIdGenerator)
        {
            _sessionStorage = sessionStorage;
            _cookieSettings = cookieSettings;
            _timeoutSettings = timeoutSettings;
            _sessionIdGenerator = sessionIdGenerator;
        }

        public CompositionRootBuilder SessionStorage(ISessionStorage sessionStorage)
        {
            return ShallowClone(sessionStorage: sessionStorage);
        }

        public CompositionRootBuilder CookieSettings(CookieSettings cookieSettings)
        {
            return ShallowClone(cookieSettings: cookieSettings);
        }

        public CompositionRootBuilder TimeoutSettings(TimeoutSettings timeoutSettings)
        {
            return ShallowClone(timeoutSettings: timeoutSettings);
        }

        public CompositionRootBuilder SessionIdGenerator(ISessionIdGenerator sessionIdGenerator)
        {
            return ShallowClone(sessionIdGenerator: sessionIdGenerator);
        }

        public CompositionRoot Build()
        {
            return new CompositionRoot(
                _sessionStorage ?? DefaultSessionStorage(),
                _cookieSettings ?? DefaultCookieSettings(),
                _timeoutSettings ?? DefaultTimeoutSettings(),
                _sessionIdGenerator ?? DefaultSessionIdGenerator());
        }

        private CompositionRootBuilder ShallowClone(
            ISessionStorage? sessionStorage = null,
            CookieSettings? cookieSettings = null,
            TimeoutSettings? timeoutSettings = null,
            ISessionIdGenerator? sessionIdGenerator = null)
        {
            return new CompositionRootBuilder(
                sessionStorage ?? _sessionStorage,
                cookieSettings ?? _cookieSettings,
                timeoutSettings ?? _timeoutSettings,
                sessionIdGenerator ?? _sessionIdGenerator);
        }
        
        private static ISessionStorage DefaultSessionStorage()
        {
            throw new NotImplementedException();
        }

        private static ISessionIdGenerator DefaultSessionIdGenerator()
        {
            throw new NotImplementedException();
        }

        private static CookieSettings DefaultCookieSettings()
        {
            return new CookieSettingsBuilder().Build();
        }

        private static TimeoutSettings DefaultTimeoutSettings()
        {
            throw new NotImplementedException();
        }
    }
}
