using System;
using System.Threading.Tasks;
using Messerli.Session.AspNetCore.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Messerli.Session.AspNetCore
{
    public class SessionMiddleware
    {
        public delegate ISessionLifecycleHandler CreateSessionLifecycleHandler();

        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        private readonly CreateSessionLifecycleHandler _createSessionLifecycleHandler;

        public SessionMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            CreateSessionLifecycleHandler sessionLifecycleHandler)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SessionMiddleware>();
            _createSessionLifecycleHandler = sessionLifecycleHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            var lifecycleHandler = _createSessionLifecycleHandler();

            var request = new Request(context);
            var response = new Response(context);

            await lifecycleHandler.OnRequest(request);

            RegisterOnResponseStartingHandler(context, async () =>
            {
                await lifecycleHandler.OnResponse(request, response);
            });

            context.Features.Set(lifecycleHandler.Session);

            try
            {
                await _next(context);
            }
            finally
            {
                context.Features[typeof(ISession)] = null;
            }
        }

        private void RegisterOnResponseStartingHandler(
            HttpContext context,
            Func<Task> onResponseAction)
        {
            context.Response.OnStarting(async () =>
            {
                try
                {
                    await onResponseAction();
                }
                catch (Exception exception)
                {
                    _logger.ErrorSavingTheSession(exception);
                }
            });
        }
    }
}
