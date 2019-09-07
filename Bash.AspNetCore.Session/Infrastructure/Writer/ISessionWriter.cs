using System.Threading.Tasks;

namespace Bash.AspNetCore.Session.Infrastructure.Writer
{
    public interface ISessionWriter
    {
        Task WriteSession(InternalSession session);
    }
}
