﻿using Messerli.Session.SessionState;
using Messerli.Session.Storage;

namespace Messerli.Session.Internal
{
    [Equals(DoNotAddEqualityOperators = true)]
    internal class RawSession
    {
        public RawSession(
            ISessionStateVariant state,
            SessionData sessionData)
        {
            State = state;
            SessionData = sessionData;
        }

        public ISessionStateVariant State { get; set; }

        public SessionData SessionData { get; set; }

        public bool ReadOnly { get; set; }

        public bool IsEmpty() => SessionData.Data.Count == 0;
    }
}
