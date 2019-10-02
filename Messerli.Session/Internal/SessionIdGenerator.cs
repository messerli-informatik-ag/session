using System;
using System.Security.Cryptography;

namespace Messerli.Session.Internal
{
    internal class SessionIdGenerator : ISessionIdGenerator
    {
        /// <summary>
        /// This number is quadruple of what is recommended as minimum by
        /// <a href="https://cheatsheetseries.owasp.org/cheatsheets/Session_Management_Cheat_Sheet.html#session-id-length">OWASP</a>.
        /// </summary>
        private const int SessionIdByteLength = 64;

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
            var bytes = new byte[SessionIdByteLength];
            using var crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(bytes);
            return bytes;
        }
    }
}
