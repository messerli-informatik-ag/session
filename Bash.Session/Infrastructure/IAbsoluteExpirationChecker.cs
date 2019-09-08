using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    internal interface IAbsoluteExpirationChecker
    {
        Task InvalidateSessionIfAbsoluteExpirationReached();
    }
}
