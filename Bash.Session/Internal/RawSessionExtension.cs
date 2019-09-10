using System;

namespace Bash.Session.Internal
{
    internal static class RawSessionExtension
    {
        public static SessionId GetId(this RawSession session)
        {
            return session.State.Map(
                mapNew: state => state.Id,
                mapExisting: state => state.Id,
                mapExistingWithNewId: state => state.NewId,
                mapAbandoned: state => throw new InvalidOperationException("Trying to retrieve the id of an abandoned session"));
        }
    }
}
