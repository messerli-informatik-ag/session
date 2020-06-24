using System;

namespace Messerli.Session.Internal
{
    internal static class RawSessionExtension
    {
        public static SessionId GetId(this RawSession session)
            => session.State.Match(
                   @new: state => state.Id,
                   existing: state => state.Id,
                   existingWithNewId: state => state.NewId,
                   abandoned: _ => throw new InvalidOperationException("Trying to retrieve the id of an abandoned session"));
    }
}
