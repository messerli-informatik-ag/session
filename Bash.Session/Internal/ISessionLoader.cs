using System.Threading.Tasks;
using Bash.Session.Http;

namespace Bash.Session.Internal
{
    internal interface ISessionLoader
    {
        Task<RawSession?> LoadFromRequest(IRequest request);
    }
}
