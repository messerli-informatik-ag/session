using System;
using Microsoft.AspNetCore.Builder;

namespace Bash.Session.AspNetCore
{
    public static class SessionMiddlewareExtension
    {
        public static IApplicationBuilder UseSession(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseSession(builder => builder);
        }

        public static IApplicationBuilder UseSession(
            this IApplicationBuilder applicationBuilder,
            Func<CompositionRootBuilder, CompositionRootBuilder> customizeCompositionRoot)
        {
            var sessionLifecycleHandler = customizeCompositionRoot(new CompositionRootBuilder())
                .Build()
                .CreateSessionLifeCycleHandler();
            return applicationBuilder.UseMiddleware<SessionMiddleware>(sessionLifecycleHandler);
        }
    }
}
