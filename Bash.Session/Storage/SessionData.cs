using System;
using System.Collections.Generic;

namespace Bash.Session.Storage
{
    [Serializable]
    public sealed class SessionData
    {
        public DateTime Created { get; }

        public IDictionary<string, byte[]> Data { get; }

        public SessionData(DateTime created)
        {
            Created = created;
            Data = new Dictionary<string, byte[]>();
        }

        public SessionData(
            DateTime created,
            IDictionary<string, byte[]> data)
        {
            Created = created;
            Data = data;
        }
    }
}
