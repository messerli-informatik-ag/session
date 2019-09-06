namespace Bash.AspNetCore.Session.SessionState
{
    public interface IVisitor
    {
        void VisitNew(New state);

        void VisitExisting(Existing state);

        void VisitExistingWithNewId(ExistingWithNewId state);
    }
}
