using System;
using System.Text.RegularExpressions;

namespace Messerli.Session.Configuration
{
    public sealed class CookieSettingsBuilder
    {
        private const CookieSecurePreference DefaultCookieSecurePreference =
            CookieSecurePreference.MatchingRequest;

        private static readonly CookieName DefaultCookieName =
            new CookieName("session_id");

        private const bool DefaultHttpOnly = true;

        private const CookieSameSiteMode DefaultSameSiteMode = CookieSameSiteMode.Lax;

        private const string ValidCookieNameRegex = @"^[a-zA-Z0-9!#$%&'*+\-.\^_`|~]+$";

        private readonly CookieName? _name;

        private readonly bool? _httpOnly;

        private readonly CookieSecurePreference? _securePreference;

        private readonly CookieSameSiteMode? _sameSiteMode;

        public CookieSettingsBuilder()
        {
        }

        private CookieSettingsBuilder(
            CookieName? name,
            bool? httpOnly,
            CookieSecurePreference? securePreference,
            CookieSameSiteMode? sameSiteMode)
        {
            _name = name;
            _httpOnly = httpOnly;
            _securePreference = securePreference;
            _sameSiteMode = sameSiteMode;
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

        public CookieSettingsBuilder SameSiteMode(CookieSameSiteMode sameSiteMode)
        {
            return ShallowClone(sameSiteMode: sameSiteMode);
        }

        /// <exception cref="InvalidOperationException">When the provided cookie name is not valid. <see cref="CookieName" /></exception>
        public CookieSettings Build()
        {
            var name = _name ?? DefaultCookieName;

            ValidateCookieName(name);

            return new CookieSettings(
                name,
                _httpOnly ?? DefaultHttpOnly,
                _securePreference ?? DefaultCookieSecurePreference,
                _sameSiteMode ?? DefaultSameSiteMode);
        }

        private static void ValidateCookieName(CookieName name)
        {
            if (!Regex.IsMatch(name.Value, ValidCookieNameRegex))
            {
                throw new InvalidOperationException("Cookie name must only contain letters, numbers, and the following characters: !#$%&'*+-.^_`|~");
            }
        }

        private CookieSettingsBuilder ShallowClone(
            CookieName? name = null,
            bool? httpOnly = null,
            CookieSecurePreference? securePreference = null,
            CookieSameSiteMode? sameSiteMode = null)
        {
            return new CookieSettingsBuilder(
                name ?? _name,
                httpOnly ?? _httpOnly,
                securePreference ?? _securePreference,
                sameSiteMode ?? _sameSiteMode);
        }
    }
}
