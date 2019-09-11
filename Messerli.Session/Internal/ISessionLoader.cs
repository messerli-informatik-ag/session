using System.Threading.Tasks;
using Messerli.Session.Http;

namespace Messerli.Session.Internal
{
    internal interface ISessionLoader
    {
        Task<RawSession?> LoadFromRequest(IRequest request);
    }
}
