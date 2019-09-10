using System;

namespace Bash.Session.SessionState
{
    [Equals(DoNotAddEqualityOperators = true)]
    internal class Existing : ISessionStateVariant
    {
        public Existing(SessionId id)
        {
            Id = id;
        }

        public SessionId Id { get; }

        public T Map<T>(
            Func<New, T> mapNew,
            Func<Existing, T> mapExisting,
            Func<ExistingWithNewId, T> mapExistingWithNewId,
            Func<Abandoned, T> mapAbandoned) => mapExisting(this);

        public void Map(
            Action<New> mapNew,
            Action<Existing> mapExisting,
            Action<ExistingWithNewId> mapExistingWithNewId,
            Action<Abandoned> mapAbandoned) => mapExisting(this);
    }
}
