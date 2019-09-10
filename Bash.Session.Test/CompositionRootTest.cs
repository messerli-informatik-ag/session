using Xunit;

namespace Bash.Session.Test
{
    public sealed class CompositionRootTest
    {
        [Fact]
        public void SessionLifecycleHandlerCanBeCreated()
        {
            var _ = new CompositionRootBuilder()
                .Build()
                .CreateSessionLifeCycleHandler();
        }
    }
}
