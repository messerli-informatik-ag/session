#pragma warning disable 660,661

namespace Messerli.Session
{
    [Equals]
    public sealed class SessionId
    {
        public SessionId(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static bool operator ==(SessionId left, SessionId right) => Operator.Weave(left, right);

        public static bool operator !=(SessionId left, SessionId right) => Operator.Weave(left, right);

        public override string ToString() => Value;
    }
}
