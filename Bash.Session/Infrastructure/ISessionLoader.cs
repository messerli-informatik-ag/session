using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    public interface ISessionLoader
    {
        Task<InternalSession?> LoadFromRequest(IRequest request);
    }
}
