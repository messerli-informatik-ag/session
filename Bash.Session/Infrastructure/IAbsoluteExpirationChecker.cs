using System.Threading.Tasks;

namespace Bash.Session.Infrastructure
{
    public interface IAbsoluteExpirationChecker
    {
        Task InvalidateSessionIfAbsoluteExpirationReached();
    }
}
