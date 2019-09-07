using System.Threading.Tasks;
using Bash.Session.Infrastructure;

namespace Bash.Session
{
    public interface ISessionLifecycleHandler
    {
        ISession Session { get; }
        
        Task OnRequest(IRequest request);

        Task OnResponse(IResponse response);
    }
}
