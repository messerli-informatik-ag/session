using System.Threading.Tasks;
using Bash.AspNetCore.Session.SessionState;

namespace Bash.AspNetCore.Session.Infrastructure.Writer
{
    public interface IOneTimeSessionWriter : IVisitor<Task>
    {
    }
}
