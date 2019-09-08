#pragma warning disable 660,661

namespace Bash.Session
{
    [Equals]
    public sealed class SessionId
    {
        public string Value { get; }

        public SessionId(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;

        public static bool operator ==(SessionId left, SessionId right) => Operator.Weave(left, right);
        
        public static bool operator !=(SessionId left, SessionId right) => Operator.Weave(left, right);
    }
}
