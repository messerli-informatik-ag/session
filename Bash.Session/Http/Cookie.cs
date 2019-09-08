using System;
using Bash.Session.Configuration;

namespace Bash.Session.Http
{
    public sealed class Cookie
    {
        public CookieSettings Settings { get; }

        public string Value { get; }

        public DateTime Expiration { get; }

        public Cookie(CookieSettings settings, string value, DateTime expiration)
        {
            Settings = settings;
            Value = value;
            Expiration = expiration;
        }
    }
}
