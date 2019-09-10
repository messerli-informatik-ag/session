using System;
using System.Text;
using Bash.Session.Utility;

namespace Bash.Session
{
    public static class SessionExtension
    {
        public static int? GetInt32(this ISession session, string key)
        {
            return NullableValue.Map<byte[], int>(session.Get(key), bytes => BitConverter.ToInt32(bytes));
        }

        public static void SetInt32(this ISession session, string key, int value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static string? GetString(this ISession session, string key)
        {
            return NullableValue.Map(session.Get(key), Encoding.UTF8.GetString);
        }

        public static void SetString(this ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
        }
    }
}
