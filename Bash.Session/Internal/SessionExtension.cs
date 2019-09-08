namespace Bash.Session.Internal
{
    internal static class SessionExtension
    {
        public static SessionId GetId(this RawSession session)
        {
            return session.State.Map(
                mapNew: state => state.Id,
                mapExisting: state => state.Id,
                mapExistingWithNewId: state => state.NewId,
                mapAbandoned: state => state.Id);
        }
    }
}
