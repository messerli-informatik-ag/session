#pragma warning disable 660,661

namespace Messerli.Session.Configuration
{
    [Equals]
    public sealed class CookieSettings
    {
        internal CookieSettings(
            CookieName name,
            bool httpOnly,
            CookieSecurePreference securePreference,
            CookieSameSiteMode sameSiteMode)
        {
            Name = name;
            HttpOnly = httpOnly;
            SecurePreference = securePreference;
            SameSiteMode = sameSiteMode;
        }

        public CookieName Name { get; }

        public bool HttpOnly { get; }

        public CookieSecurePreference SecurePreference { get; }

        public CookieSameSiteMode SameSiteMode { get; }

        public static bool operator ==(CookieSettings left, CookieSettings right) => Operator.Weave(left, right);

        public static bool operator !=(CookieSettings left, CookieSettings right) => Operator.Weave(left, right);
    }
}
