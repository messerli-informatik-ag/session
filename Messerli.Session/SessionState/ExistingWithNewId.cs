using System;

namespace Messerli.Session.SessionState
{
    [Equals(DoNotAddEqualityOperators = true)]
    internal class ExistingWithNewId : ISessionStateVariant
    {
        public ExistingWithNewId(SessionId oldId, SessionId newId)
        {
            OldId = oldId;
            NewId = newId;
        }

        public SessionId OldId { get; }

        public SessionId NewId { get; }

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
