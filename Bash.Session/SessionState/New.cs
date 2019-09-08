using System;

namespace Bash.Session.SessionState
{
    internal class New : ISessionStateVariant
    {
        public New(SessionId id)
        {
            Id = id;
        }

        public SessionId Id { get; }

        public T Map<T>(
            Func<New, T> mapNew,
            Func<Existing, T> mapExisting,
            Func<ExistingWithNewId, T> mapExistingWithNewId,
            Func<Abandoned, T> mapAbandoned) => mapNew(this);

        public void Map(
            Action<New> mapNew,
            Action<Existing> mapExisting,
            Action<ExistingWithNewId> mapExistingWithNewId,
            Action<Abandoned> mapAbandoned) => mapNew(this);
    }
}
