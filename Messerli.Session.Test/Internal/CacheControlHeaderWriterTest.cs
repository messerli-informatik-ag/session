using System;
using Messerli.Session.Http;
using Messerli.Session.Internal;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public class CacheControlHeaderWriterTest
    {
        private const string ExpectedCacheControlHeader = "must-revalidate, max-age=0, private";

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
                .Setup(r => r.GetFirstHeaderValue(It.IsAny<string>()))
                .Returns(() => null);
            response
                .Setup(r => r.SetHeader(It.IsAny<string>(), ExpectedCacheControlHeader));
            var cacheControlHeaderWriter = CreateCacheControlHeaderWriter();
            cacheControlHeaderWriter.AddCacheControlHeaders(response.Object);
        }

        [Fact]
        public void ThrowsIfCacheControlHeaderIsAlreadyPresentInResponse()
        {
            const string validCacheControlHeaderValue = "private, must-revalidate";
            var response = new Mock<IResponse>(MockBehavior.Strict);
            response
                .Setup(r => r.AutomaticCacheControl)
                .Returns(true);
            response.Setup(r => r.GetFirstHeaderValue(It.IsAny<string>()))
                .Returns(validCacheControlHeaderValue);
            var cacheControlHeaderWriter = CreateCacheControlHeaderWriter();

            Assert.Throws<InvalidOperationException>(() =>
            {
                cacheControlHeaderWriter.AddCacheControlHeaders(response.Object);
            });
        }

        [Fact]
        public void DoesNothingWhenCachingIsAlreadyDisabled()
        {
            const string cacheControlNoCacheValue = "no-cache";
            var response = new Mock<IResponse>(MockBehavior.Strict);
            response
                .Setup(r => r.AutomaticCacheControl)
                .Returns(true);
            response.Setup(r => r.GetFirstHeaderValue(It.IsAny<string>()))
                .Returns(cacheControlNoCacheValue);
            var cacheControlHeaderWriter = CreateCacheControlHeaderWriter();
            cacheControlHeaderWriter.AddCacheControlHeaders(response.Object);
        }

        private static ICacheControlHeaderWriter CreateCacheControlHeaderWriter()
        {
            return new CacheControlHeaderWriter();
        }
    }
}
