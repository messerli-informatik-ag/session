using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    internal interface ISessionLoader
    {
        Task<RawSession?> LoadFromRequest(IRequest request);
    }
}
