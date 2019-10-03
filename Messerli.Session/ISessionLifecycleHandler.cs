using System.Threading.Tasks;
using Messerli.Session.Http;

namespace Messerli.Session
{
    public interface ISessionLifecycleHandler
    {
        ISession Session { get; }

        Task OnRequest(IRequest request);

        Task OnResponse(IRequest request, IResponse response);
    }
}
