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

        public T Match<T>(
            Func<New, T> @new,
            Func<Existing, T> existing,
            Func<ExistingWithNewId, T> existingWithNewId,
            Func<Abandoned, T> abandoned) => existingWithNewId(this);

        public void Match(
            Action<New> @new,
            Action<Existing> existing,
            Action<ExistingWithNewId> existingWithNewId,
            Action<Abandoned> abandoned) => existingWithNewId(this);
    }
}
