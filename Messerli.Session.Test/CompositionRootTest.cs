using Xunit;

namespace Messerli.Session.Test
{
    public sealed class CompositionRootTest
    {
        [Fact]
        public void SessionLifecycleHandlerCanBeCreated()
        {
            var unusedResult = new CompositionRootBuilder()
                .Build()
                .CreateSessionLifeCycleHandler();
        }
    }
}
