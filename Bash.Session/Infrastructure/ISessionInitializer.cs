using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    internal interface ISessionInitializer
    {
        Task<RawSession> InitializeSession(IRequest request);
    }
}
