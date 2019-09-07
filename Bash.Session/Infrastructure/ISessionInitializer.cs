using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    public interface ISessionInitializer
    {
        Task<InternalSession> InitializeSession(IRequest request);
    }
}
