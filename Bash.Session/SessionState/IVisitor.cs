namespace Bash.Session.SessionState
{
    public interface IVisitor<out T>
    {
        T VisitNew(New state);

        T VisitExisting(Existing state);

        T VisitExistingWithNewId(ExistingWithNewId state);

        T VisitAbandoned(Abandoned abandoned);
    }
}
