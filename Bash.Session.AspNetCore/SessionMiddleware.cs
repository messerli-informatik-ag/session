using System.Threading.Tasks;
using Bash.Session.AspNetCore.Internal;
using Microsoft.AspNetCore.Http;

namespace Bash.Session.AspNetCore
{
    public class SessionMiddleware : IMiddleware
    {
        public delegate ISessionLifecycleHandler CreateSessionLifecycleHandler();

        private readonly CreateSessionLifecycleHandler _createSessionLifecycleHandler;

        public SessionMiddleware(CreateSessionLifecycleHandler sessionLifecycleHandler)
        {
            _createSessionLifecycleHandler = sessionLifecycleHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var lifecycleHandler = _createSessionLifecycleHandler();

            var request = new Request(context);
            var response = new Response(context);

            await lifecycleHandler.OnRequest(request);
            context.Features.Set(lifecycleHandler.Session);

            // TODO: handle errors properly

            try
            {
                await next(context);
            }
            finally
            {
                context.Features.Set<ISession>(null);
                await lifecycleHandler.OnResponse(response);
            }
        }
    }
}
