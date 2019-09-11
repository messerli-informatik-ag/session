using System.Threading.Tasks;
using Messerli.Session.AspNetCore.Internal;
using Microsoft.AspNetCore.Http;

namespace Messerli.Session.AspNetCore
{
    public class SessionMiddleware
    {
        public delegate ISessionLifecycleHandler CreateSessionLifecycleHandler();

        private readonly CreateSessionLifecycleHandler _createSessionLifecycleHandler;

        private readonly RequestDelegate _next;

        public SessionMiddleware(
            RequestDelegate next,
            CreateSessionLifecycleHandler sessionLifecycleHandler)
        {
            _next = next;
            _createSessionLifecycleHandler = sessionLifecycleHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            // TODO: handle errors properly

            var lifecycleHandler = _createSessionLifecycleHandler();

            var request = new Request(context);
            var response = new Response(context);

            await lifecycleHandler.OnRequest(request);
            context.Features.Set(lifecycleHandler.Session);
            
            context.Response.OnStarting(async () =>
            {
                await lifecycleHandler.OnResponse(request, response); 
            });

            try
            {
                await _next(context);
            }
            finally
            {
                context.Features[typeof(ISession)] = null;
            }
        }
    }
}
