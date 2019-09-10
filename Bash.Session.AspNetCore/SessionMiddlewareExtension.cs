using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;

namespace Bash.Session.AspNetCore
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
            return applicationBuilder.UseMiddleware<SessionMiddleware>(createLifecycleHandler);
        }

        private static CompositionRoot CreateCompositionRoot(
            IApplicationBuilder applicationBuilder,
            ConfigureCompositionRoot configureCompositionRoot)
        {
            return configureCompositionRoot(new CompositionRootBuilder())
                .SessionStorage(new Internal.Storage(GetDistributedCache(applicationBuilder)))
                .Build();
        }

        private static IDistributedCache GetDistributedCache(IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder
                       .ApplicationServices
                       .GetService(typeof(IDistributedCache)) as IDistributedCache
                   ?? throw new NullReferenceException($"Unable to resolve {nameof(IDistributedCache)}");
        }
    }
}
