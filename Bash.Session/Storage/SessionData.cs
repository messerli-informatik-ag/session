using System;
using System.Collections.Generic;

namespace Bash.Session.Storage
{
    [Serializable]
    [Equals(DoNotAddEqualityOperators = true)]
    public sealed class SessionData
    {
        public DateTime CreationDate { get; }

        public IDictionary<string, byte[]> Data { get; }

        public SessionData(DateTime creationDate) : this(creationDate, new Dictionary<string, byte[]>())
        {
        }

        public SessionData(
            DateTime creationDate,
            IDictionary<string, byte[]> data)
        {
            CreationDate = creationDate;
            Data = data;
        }
    }
}
