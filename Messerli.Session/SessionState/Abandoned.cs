using System;

namespace Messerli.Session.SessionState
{
    [Equals(DoNotAddEqualityOperators = true)]
    internal class Abandoned : ISessionStateVariant
    {
        public Abandoned(SessionId? id)
        {
            Id = id;
        }

        /// <summary>
        /// The id of an existing session.
        /// </summary>
        public SessionId? Id { get; }

        public T Map<T>(
            Func<New, T> mapNew,
            Func<Existing, T> mapExisting,
            Func<ExistingWithNewId, T> mapExistingWithNewId,
            Func<Abandoned, T> mapAbandoned) => mapAbandoned(this);

        public void Map(
            Action<New> mapNew,
            Action<Existing> mapExisting,
            Action<ExistingWithNewId> mapExistingWithNewId,
            Action<Abandoned> mapAbandoned) => mapAbandoned(this);
    }
}