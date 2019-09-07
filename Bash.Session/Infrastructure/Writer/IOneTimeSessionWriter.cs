using System.Threading.Tasks;
using Bash.Session.SessionState;

namespace Bash.Session.Infrastructure.Writer
{
    public interface IOneTimeSessionWriter : IVisitor<Task>
    {
    }
}
