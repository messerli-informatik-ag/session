using System.Threading.Tasks;

namespace Bash.Session.Internal
{
    internal interface IAbsoluteExpirationChecker
    {
        Task InvalidateSessionIfAbsoluteExpirationReached();
    }
}
