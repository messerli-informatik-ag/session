using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Messerli.Session.Internal;

namespace Messerli.Session.Storage
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

        public Task<SessionData?> ReadSessionData(SessionId id)
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

            if (!entry.IsExpired(_dateTimeFactory.Now()))
            {
                return entry;
            }

            _storage.Remove(entry.Id);
            return null;
        }

        private class Entry
        {
            internal SessionId Id { get; }
            
            internal SessionData SessionData { get; }

            private DateTime IdleExpirationDate { get; }

            internal Entry(SessionId id, SessionData sessionData, DateTime idleExpirationDate)
            {
                Id = id;
                SessionData = sessionData;
                IdleExpirationDate = idleExpirationDate;
            }

            internal bool IsExpired(DateTime now)
            {
                return now > IdleExpirationDate;
            }
        }
    }
}
