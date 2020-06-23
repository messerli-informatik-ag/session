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

        public T Match<T>(
            Func<New, T> @new,
            Func<Existing, T> existing,
            Func<ExistingWithNewId, T> existingWithNewId,
            Func<Abandoned, T> abandoned) => abandoned(this);

        public void Match(
            Action<New> @new,
            Action<Existing> existing,
            Action<ExistingWithNewId> existingWithNewId,
            Action<Abandoned> abandoned) => abandoned(this);
    }
}
