#pragma warning disable 660,661

namespace Messerli.Session.Configuration
{
    /// <summary>
    /// The name of a cookie.
    /// It should only contain letters, numbers, and the following characters: <pre>!#$%&amp;'*+-.^_`|~</pre>.
    /// </summary>
    [Equals]
    public sealed class CookieName
    {
        public CookieName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static bool operator ==(CookieName left, CookieName right) => Operator.Weave(left, right);

        public static bool operator !=(CookieName left, CookieName right) => Operator.Weave(left, right);

        public override string ToString() => Value;
    }
}
