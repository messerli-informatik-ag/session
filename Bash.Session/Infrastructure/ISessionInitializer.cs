using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    public interface ISessionInitializer
    {
        Task<RawSession> InitializeSession(IRequest request);
    }
}
