namespace Messerli.Session.Configuration
{
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
    }
}
