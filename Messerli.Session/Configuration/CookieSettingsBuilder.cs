using System;

namespace Messerli.Session.Configuration
{
    public sealed class CookieSettingsBuilder
    {
        private const CookieSecurePreference DefaultCookieSecurePreference =
            CookieSecurePreference.MatchingRequest;

        private static readonly CookieName DefaultCookieName =
            new CookieName("session_id");

        private static readonly bool DefaultHttpOnly = true;

        private readonly CookieName? _name;

        private readonly bool? _httpOnly;

        private readonly CookieSecurePreference? _securePreference;

        public CookieSettingsBuilder()
        {
        }

        private CookieSettingsBuilder(
            CookieName? name,
            bool? httpOnly,
            CookieSecurePreference? securePreference)
        {
            _name = name;
            _httpOnly = httpOnly;
            _securePreference = securePreference;
        }

        public CookieSettingsBuilder Name(CookieName name)
        {
            return ShallowClone(name: name);
        }

        public CookieSettingsBuilder HttpOnly(bool httpOnly)
        {
            return ShallowClone(httpOnly: httpOnly);
        }

        public CookieSettingsBuilder SecurePreference(CookieSecurePreference securePreference)
        {
            return ShallowClone(securePreference: securePreference);
        }

        public CookieSettings Build()
        {
            var name = _name ?? DefaultCookieName;

            ValidateCookieName(name);

            return new CookieSettings(
                name,
                _httpOnly ?? DefaultHttpOnly,
                _securePreference ?? DefaultCookieSecurePreference);
        }

        private static void ValidateCookieName(CookieName name)
        {
            if (string.IsNullOrWhiteSpace(name.Value))
            {
                throw new InvalidOperationException("Cookie name must not be contain whitespace only");
            }
        }

        private CookieSettingsBuilder ShallowClone(
            CookieName? name = null,
            bool? httpOnly = null,
            CookieSecurePreference? securePreference = null)
        {
            return new CookieSettingsBuilder(
                name ?? _name,
                httpOnly ?? _httpOnly,
                securePreference ?? _securePreference);
        }
    }
}
