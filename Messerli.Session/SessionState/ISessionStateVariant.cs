using System;

namespace Messerli.Session.SessionState
{
    internal interface ISessionStateVariant
    {
        T Match<T>(
            Func<New, T> @new,
            Func<Existing, T> existing,
            Func<ExistingWithNewId, T> existingWithNewId,
            Func<Abandoned, T> abandoned);

        void Match(
            Action<New> @new,
            Action<Existing> existing,
            Action<ExistingWithNewId> existingWithNewId,
            Action<Abandoned> abandoned);
    }
}
