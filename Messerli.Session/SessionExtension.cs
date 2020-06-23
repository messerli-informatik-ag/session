using System;
using System.Text;
using Messerli.Session.Utility;

namespace Messerli.Session
{
    public static class SessionExtension
    {
        public static int? GetInt32(this ISession session, string key)
            => NullableValue.Map<byte[], int>(session.Get(key), bytes => BitConverter.ToInt32(bytes));

        public static void SetInt32(this ISession session, string key, int value)
            => session.Set(key, BitConverter.GetBytes(value));

        public static string? GetString(this ISession session, string key)
            => NullableValue.Map(session.Get(key), Encoding.UTF8.GetString);

        public static void SetString(this ISession session, string key, string value)
            => session.Set(key, Encoding.UTF8.GetBytes(value));

        public static bool? GetBoolean(this ISession session, string key)
            => NullableValue.Map<byte[], bool>(session.Get(key), bytes => BitConverter.ToBoolean(bytes));

        public static void SetBoolean(this ISession session, string key, bool value)
            => session.Set(key, BitConverter.GetBytes(value));
    }
}
