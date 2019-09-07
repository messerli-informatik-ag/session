using System.Threading.Tasks;

namespace Bash.AspNetCore.Session.Infrastructure
{
    public interface IAbsoluteExpirationChecker
    {
        Task InvalidateSessionIfAbsoluteExpirationReached();
    }
}
