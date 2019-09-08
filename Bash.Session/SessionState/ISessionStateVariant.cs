using System;

namespace Bash.Session.SessionState
{
    internal interface ISessionStateVariant
    {
        T Map<T>(
            Func<New, T> mapNew,
            Func<Existing, T> mapExisting,
            Func<ExistingWithNewId, T> mapExistingWithNewId,
            Func<Abandoned, T> mapAbandoned);

        void Map(
            Action<New> mapNew,
            Action<Existing> mapExisting,
            Action<ExistingWithNewId> mapExistingWithNewId,
            Action<Abandoned> mapAbandoned);
    }
}
