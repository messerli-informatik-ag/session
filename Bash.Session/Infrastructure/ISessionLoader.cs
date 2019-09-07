using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    public interface ISessionLoader
    {
        Task<RawSession?> LoadFromRequest(IRequest request);
    }
}
