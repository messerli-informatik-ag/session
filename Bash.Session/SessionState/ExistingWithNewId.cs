using System;

namespace Bash.Session.SessionState
{
    internal class ExistingWithNewId : ISessionStateVariant
    {
        public SessionId OldId { get; }

        public SessionId NewId { get; }

        public ExistingWithNewId(SessionId oldId, SessionId newId)
        {
            OldId = oldId;
            NewId = newId;
        }

        public T Map<T>(
            Func<New, T> mapNew,
            Func<Existing, T> mapExisting,
            Func<ExistingWithNewId, T> mapExistingWithNewId,
            Func<Abandoned, T> mapAbandoned) => mapExistingWithNewId(this);

        public void Map(
            Action<New> mapNew,
            Action<Existing> mapExisting,
            Action<ExistingWithNewId> mapExistingWithNewId,
            Action<Abandoned> mapAbandoned) => mapExistingWithNewId(this);
    }
}
