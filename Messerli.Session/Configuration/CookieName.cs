#pragma warning disable 660,661

namespace Messerli.Session.Configuration
{
    [Equals]
    public sealed class CookieName
    {
        public string Value { get; }

        public CookieName(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;
        
        public static bool operator ==(CookieName left, CookieName right) => Operator.Weave(left, right);
        
        public static bool operator !=(CookieName left, CookieName right) => Operator.Weave(left, right);
    }
}
