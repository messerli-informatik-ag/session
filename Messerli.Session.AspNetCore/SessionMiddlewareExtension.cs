using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Messerli.Session.AspNetCore
{
    public static class SessionMiddlewareExtension
    {
        public delegate CompositionRootBuilder ConfigureCompositionRoot(CompositionRootBuilder compositionRootBuilder);

        public static IApplicationBuilder UseSession(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseSession(builder => builder);
        }

        public static IApplicationBuilder UseSession(
            this IApplicationBuilder applicationBuilder,
            ConfigureCompositionRoot configureCompositionRoot)
        {
            var compositionRoot = CreateCompositionRoot(applicationBuilder, configureCompositionRoot);
            var createLifecycleHandler = new SessionMiddleware.CreateSessionLifecycleHandler(
                compositionRoot.CreateSessionLifeCycleHandler);
            var loggerFactory = ResolveApplicationService<ILoggerFactory>(applicationBuilder);
            var logger = loggerFactory.CreateLogger<SessionMiddleware>();
            return applicationBuilder.UseMiddleware<SessionMiddleware>(logger, createLifecycleHandler);
        }

        private static CompositionRoot CreateCompositionRoot(
            IApplicationBuilder applicationBuilder,
            ConfigureCompositionRoot configureCompositionRoot)
        {
            var distributedCache = ResolveApplicationService<IDistributedCache>(applicationBuilder);
            return configureCompositionRoot(new CompositionRootBuilder())
                .SessionStorage(new Internal.Storage(distributedCache))
                .Build();
        }

        private static T ResolveApplicationService<T>(IApplicationBuilder applicationBuilder)
            where T : class
        {
            var type = typeof(T);
            return applicationBuilder.ApplicationServices.GetService(type) as T
                   ?? throw new NullReferenceException($"Unable to resolve {type.Name}");
        }
    }
}
