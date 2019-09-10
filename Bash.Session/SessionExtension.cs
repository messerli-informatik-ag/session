using System;
using System.Text;

namespace Bash.Session
{
    public static class SessionExtension
    {
        public static int? GetInt32(this ISession session, string key)
        {
            return Map<int>(session.Get(key), bytes => BitConverter.ToInt32(bytes));
        }

        public static void SetInt32(this ISession session, string key, int value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static string? GetString(this ISession session, string key)
        {
            return Map(session.Get(key), Encoding.UTF8.GetString);
        }

        public static void SetString(this ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
        }

        public static TOutput? Map<TOutput>(byte[]? input, Func<byte[], TOutput?> transform)
            where TOutput: class
        {
            return input is { } notNullInput
                ? transform(notNullInput)
                : null;
        }

        public static TOutput? Map<TOutput>(byte[]? input, Func<byte[], TOutput?> transform)
            where TOutput: struct
        {
            return input is { } notNullInput
                ? transform(notNullInput)
                : new TOutput?();
        }
    }
}
