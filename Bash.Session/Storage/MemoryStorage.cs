using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bash.Session.Internal;

namespace Bash.Session.Storage
{
    internal class MemoryStorage : ISessionStorage
    {
        private readonly IDictionary<SessionId, Entry> _storage
            = new ConcurrentDictionary<SessionId, Entry>();

        private readonly IDateTimeFactory _dateTimeFactory;

        internal MemoryStorage(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
        }

        public Task WriteSessionData(SessionId id, SessionData data, DateTime idleExpirationDate)
        {
            return Task.Run(() =>
            {
                _storage[id] = new Entry(id, data, idleExpirationDate);
            });
        }

        public Task RemoveSessionData(SessionId id)
        {
            return Task.Run(() =>
            {
                _storage.Remove(id);
            });
        }

        public Task<SessionData> ReadSessionData(SessionId id)
        {
            return Task.Run(() =>
            {
                _storage.TryGetValue(id, out var entry);
                return CheckEntryExpiration(entry)?.SessionData;
            });
        }

        private Entry? CheckEntryExpiration(Entry? entry)
        {
            if (entry is null)
            {
                return null;
            }

            if (entry.IsExpired(_dateTimeFactory.Now()))
            {
                _storage.Remove(entry.Id);
                return null;
            }

            return entry;
        }

        private class Entry
        {
            public SessionId Id { get; }
            
            public SessionData SessionData { get; }
            
            public DateTime IdleExpirationDate { get; }

            public Entry(SessionId id, SessionData sessionData, DateTime idleExpirationDate)
            {
                Id = id;
                SessionData = sessionData;
                IdleExpirationDate = idleExpirationDate;
            }

            public bool IsExpired(DateTime now)
            {
                return now > IdleExpirationDate;
            }
        }
    }
}
