using Microsoft.AspNetCore.Builder;

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
            var compositionRoot = CreateCompositionRoot(configureCompositionRoot);
            var createLifecycleHandler = new SessionMiddleware.CreateSessionLifecycleHandler(
                compositionRoot.CreateSessionLifeCycleHandler);
            return applicationBuilder.UseMiddleware<SessionMiddleware>(createLifecycleHandler);
        }

        private static CompositionRoot CreateCompositionRoot(ConfigureCompositionRoot configureCompositionRoot)
        {
            return configureCompositionRoot(new CompositionRootBuilder()).Build();
        }
    }
}
