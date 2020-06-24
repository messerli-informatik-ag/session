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
            _storage[id] = new Entry(id, data, idleExpirationDate);
            return Task.CompletedTask;
        }

        public Task RemoveSessionData(SessionId id)
        {
            _storage.Remove(id);
            return Task.CompletedTask;
        }

        public Task<SessionData?> ReadSessionData(SessionId id)
        {
            _storage.TryGetValue(id, out var entry);
            return Task.FromResult(CheckEntryExpiration(entry)?.SessionData);
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
            internal Entry(SessionId id, SessionData sessionData, DateTime idleExpirationDate)
            {
                Id = id;
                SessionData = sessionData;
                IdleExpirationDate = idleExpirationDate;
            }

            internal SessionId Id { get; }

            internal SessionData SessionData { get; }

            private DateTime IdleExpirationDate { get; }

            internal bool IsExpired(DateTime now)
            {
                return now > IdleExpirationDate;
            }
        }
    }
}
