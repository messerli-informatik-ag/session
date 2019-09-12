#pragma warning disable 660,661

namespace Messerli.Session.Configuration
{
    [Equals]
    public sealed class CookieSettings
    {
        public CookieName Name { get; }

        public bool HttpOnly { get; }

        public CookieSecurePreference SecurePreference { get; }

        internal CookieSettings(
            CookieName name,
            bool httpOnly,
            CookieSecurePreference securePreference)
        {
            Name = name;
            HttpOnly = httpOnly;
            SecurePreference = securePreference;
        }

        public static bool operator ==(CookieSettings left, CookieSettings right) => Operator.Weave(left, right);

        public static bool operator !=(CookieSettings left, CookieSettings right) => Operator.Weave(left, right);
    }
}
