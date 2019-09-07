using System;
using System.Collections.Generic;

namespace Bash.Session.Infrastructure
{
    public class SessionData
    {
        public DateTime Created { get; }

        public IDictionary<string, string> Data { get; }

        public SessionData(DateTime created)
        {
            Created = created;
            Data = new Dictionary<string, string>();
        }

        public SessionData(
            DateTime created,
            IDictionary<string, string> data)
        {
            Created = created;
            Data = data;
        }
    }
}
