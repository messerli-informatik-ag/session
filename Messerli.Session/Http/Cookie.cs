#pragma warning disable 660,661

using System;
using Messerli.Session.Configuration;

namespace Messerli.Session.Http
{
    [Equals]
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

        public static bool operator ==(Cookie left, Cookie right) => Operator.Weave(left, right);

        public static bool operator !=(Cookie left, Cookie right) => Operator.Weave(left, right);
    }
}
