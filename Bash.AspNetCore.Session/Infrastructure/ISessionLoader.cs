using System.Threading.Tasks;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface ISessionLoader
    {
        Task<InternalSession?> LoadFromRequest(IRequest request);
    }
}
