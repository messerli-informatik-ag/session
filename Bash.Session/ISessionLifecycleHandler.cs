using System.Threading.Tasks;
using Bash.Session.Http;

namespace Bash.Session
{
    public interface ISessionLifecycleHandler
    {
        ISession Session { get; }
        
        Task OnRequest(IRequest request);

        Task OnResponse(IRequest request, IResponse response);
    }
}
