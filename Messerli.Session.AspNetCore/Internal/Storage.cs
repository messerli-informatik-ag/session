using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Messerli.Session.Storage;
using Microsoft.Extensions.Caching.Distributed;

namespace Messerli.Session.AspNetCore.Internal
{
    internal class Storage : ISessionStorage
    {
        private static readonly string CacheKeyPrefix = typeof(SessionMiddleware).Namespace;

        private readonly IDistributedCache _distributedCache;

        public Storage(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task WriteSessionData(SessionId id, SessionData data, DateTime idleExpirationDate)
        {
            await _distributedCache.SetAsync(
                CacheKey(id),
                Serialize(data),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = idleExpirationDate,
                });
        }

        public async Task RemoveSessionData(SessionId id)
        {
            await _distributedCache.RemoveAsync(CacheKey(id));
        }

        public async Task<SessionData?> ReadSessionData(SessionId id)
        {
            var data = await _distributedCache.GetAsync(CacheKey(id));

            return data is { }
                ? Deserialize(data)
                : null;
        }

        private static byte[] Serialize(SessionData data)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            return stream.ToArray();
        }

        private static SessionData? Deserialize(byte[] data)
        {
            var stream = new MemoryStream(data);
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream) as SessionData;
        }

        private static string CacheKey(SessionId id) => $"{CacheKeyPrefix}.{id}";
    }
}
