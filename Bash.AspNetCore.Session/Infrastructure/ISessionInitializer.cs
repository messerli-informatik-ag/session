using System.Threading.Tasks;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface ISessionInitializer
    {
        Task<InternalSession> InitializeSession(IRequest request);
    }
}
