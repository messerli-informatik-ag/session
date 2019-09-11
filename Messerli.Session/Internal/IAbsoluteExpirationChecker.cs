using System.Threading.Tasks;

namespace Messerli.Session.Internal
{
    internal interface IAbsoluteExpirationChecker
    {
        Task InvalidateSessionIfAbsoluteExpirationReached();
    }
}
