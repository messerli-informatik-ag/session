using Messerli.Session.Http;
using Messerli.Session.Internal;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public class CacheControlHeaderWriterTest
    {
        [Fact]
        public void DoesNothingWhenAutomaticCacheControlIsDisabledOnResponse()
        {
            var response = new Mock<IResponse>(MockBehavior.Strict);
            response
                .Setup(r => r.AutomaticCacheControl)
                .Returns(false);
            var cacheControlHeaderWriter = CreateCacheControlHeaderWriter();
            cacheControlHeaderWriter.AddCacheControlHeaders(response.Object);
        }

        [Fact]
        public void SetsHeaderWhenAutomaticCacheControlIsEnabled()
        {
            var response = new Mock<IResponse>(MockBehavior.Strict);
            response
                .Setup(r => r.AutomaticCacheControl)
                .Returns(true);
            response
                .Setup(r => r.SetHeader(It.IsAny<string>(), It.IsAny<string>()));
            var cacheControlHeaderWriter = CreateCacheControlHeaderWriter();
            cacheControlHeaderWriter.AddCacheControlHeaders(response.Object);
        }

        private static ICacheControlHeaderWriter CreateCacheControlHeaderWriter()
        {
            return new CacheControlHeaderWriter();
        }
    }
}
