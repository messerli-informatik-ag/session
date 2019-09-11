using System;
using System.Threading.Tasks;
using Messerli.Session.Configuration;
using Messerli.Session.Http;
using Messerli.Session.Internal;
using Messerli.Session.SessionState;
using Messerli.Session.Storage;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public class SessionLoaderTest
    {
        private static readonly CookieName CookieName = new CookieName("session");

        [Theory]
        [MemberData(nameof(InvalidSessionIds))]
        public async Task InvalidSessionIdsAreNotAccepted(string invalidSessionId)
        {
            var sessionStorage = new Mock<ISessionStorage>(MockBehavior.Strict);
            var request = MockRequest(invalidSessionId);
            var sessionLoader = CreateSessionLoader(sessionStorage.Object);
            Assert.Null(await sessionLoader.LoadFromRequest(request));
        }

        [Fact]
        public async Task ReturnsNothingIfNoCookieExists()
        {
            var sessionStorage = new Mock<ISessionStorage>(MockBehavior.Strict);
            var request = MockRequest(null);
            var sessionLoader = CreateSessionLoader(sessionStorage.Object);
            Assert.Null(await sessionLoader.LoadFromRequest(request));
        }

        [Fact]
        public async Task ReturnsNothingWhenNoDataIsFoundInStorage()
        {
            var sessionId = new SessionId("1234");

            var sessionStorage = new Mock<ISessionStorage>(MockBehavior.Strict);
            sessionStorage
                .Setup(s => s.ReadSessionData(sessionId))
                .ReturnsAsync(() => null);

            var request = MockRequest(sessionId.Value);

            var sessionLoader = CreateSessionLoader(sessionStorage.Object);
            Assert.Null(await sessionLoader.LoadFromRequest(request));
        }

        [Fact]
        public async Task LoadsSession()
        {
            var sessionId = new SessionId("1234");
            var sessionData = new SessionData(DateTime.UnixEpoch);
            var expectedSession = new RawSession(new Existing(sessionId), sessionData);

            var sessionStorage = new Mock<ISessionStorage>(MockBehavior.Strict);
            sessionStorage
                .Setup(s => s.ReadSessionData(sessionId))
                .ReturnsAsync(sessionData);

            var request = MockRequest(sessionId.Value);

            var sessionLoader = CreateSessionLoader(sessionStorage.Object);
            var session = await sessionLoader.LoadFromRequest(request);
            Assert.Equal(expectedSession, session);
        }

        public static TheoryData<string> InvalidSessionIds() => Constant.WhitespaceValues;

        private static ISessionLoader CreateSessionLoader(ISessionStorage sessionStorage)
        {
            return new SessionLoader(sessionStorage, CookieName);
        }

        private static IRequest MockRequest(string? cookieValue)
        {
            var request = new Mock<IRequest>(MockBehavior.Strict);
            request.Setup(r => r.GetCookie(CookieName))
                .Returns(cookieValue);
            return request.Object;
        }
    }
}
