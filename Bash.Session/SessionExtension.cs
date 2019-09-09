namespace Bash.Session
{
    public static class SessionExtension
    {
        public static int? GetInt32(this ISession session, string key)
        {
            return session.Get(key) switch
            {
                { } value => int.Parse(value),
                _ => null as int?,
            };
        }

        public static void SetInt32(this ISession session, string key, int value)
        {
            session.Set(key, value.ToString());
        }
    }
}
