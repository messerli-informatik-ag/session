using System;

namespace Messerli.Session.SessionState
{
    [Equals(DoNotAddEqualityOperators = true)]
    internal class Existing : ISessionStateVariant
    {
        public Existing(SessionId id)
        {
            Id = id;
        }

        public SessionId Id { get; }

        public T Match<T>(
            Func<New, T> @new,
            Func<Existing, T> existing,
            Func<ExistingWithNewId, T> existingWithNewId,
            Func<Abandoned, T> abandoned) => existing(this);

        public void Match(
            Action<New> @new,
            Action<Existing> existing,
            Action<ExistingWithNewId> existingWithNewId,
            Action<Abandoned> abandoned) => existing(this);
    }
}
