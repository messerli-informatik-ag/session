using System;
using System.Security.Cryptography;

namespace Bash.Session.Internal
{
    internal class SessionIdGenerator : ISessionIdGenerator
    {
        private const int SessionIdLength = 48;

        public SessionId Generate()
        {
            var id = ConvertBytesToString(GetRandomBytes());
            return new SessionId(id);
        }

        private static string ConvertBytesToString(byte[] bytes)
        {
            return BitConverter
                .ToString(bytes)
                .Replace("-", string.Empty);
        }

        private static byte[] GetRandomBytes()
        {
            var bytes = new byte[SessionIdLength];
            using var crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(bytes);
            return bytes;
        }
    }
}
