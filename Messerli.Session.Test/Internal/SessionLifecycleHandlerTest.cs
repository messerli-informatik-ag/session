using System;
using System.Threading.Tasks;
using Messerli.Session.Http;
using Messerli.Session.Internal;
using Messerli.Session.Internal.Writer;
using Messerli.Session.SessionState;
using Messerli.Session.Storage;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public class SessionLifecycleHandlerTest
    {
        private static readonly DateTime CreationDate = DateTime.UnixEpoch;

        private static readonly DateTime IdleExpirationTime = new DateTime(2000, 2, 2, 4, 4, 4);

        private static readonly SessionId SessionId = new SessionId("foo-bar");

        [Fact]
        public async Task CreatesNewSessionIfNoneCanBeLoaded()
        {
            var session = CreateSession(new New(SessionId));
            var builder = new SessionLifecycleHandlerBuilder()
                .ConfigureSessionLoader(null)
                .ConfigureSessionCreator(session);

            var lifecycleHandler = builder.Build();
            await lifecycleHandler.OnRequest(CreateMockRequest());
            Assert.Equal(
                session,
                ExtractRawSession(lifecycleHandler.Session));
        }

        [Fact]
        public async Task UsesLoadedSession()
        {
            var session = CreateSession(new Existing(SessionId));
            var builder = new SessionLifecycleHandlerBuilder()
                .ConfigureSessionLoader(session);

            var lifecycleHandler = builder.Build();
            await lifecycleHandler.OnRequest(CreateMockRequest());
            Assert.Equal(
                session,
                ExtractRawSession(lifecycleHandler.Session));
        }

        [Fact]
        public void SessionCannotBeAccessedBeforeCallingOnRequest()
        {
            var builder = new SessionLifecycleHandlerBuilder();
            var lifecycleHandler = builder.Build();

            Assert.Throws<InvalidOperationException>(() =>
            {
                var _ = lifecycleHandler.Session;
            });
        }

        [Fact]
        public async Task SessionIsWrittenToStorageAndCookie()
        {
            var session = CreateSession(new Existing(SessionId));

            var lifecycleHandlerBuilder = new SessionLifecycleHandlerBuilder()
                .ConfigureSessionLoader(session)
                .ConfigureExpirationRetriever(session)
                .ConfigureSessionWriter(session)
                .ConfigureCookieWriter(session);
            var lifecycleHandler = lifecycleHandlerBuilder.Build();

            var request = CreateMockRequest();
            var response = CreateMockResponse();

            await lifecycleHandler.OnRequest(request);
            await lifecycleHandler.OnResponse(request, response);

            lifecycleHandlerBuilder
                .SessionWriter
                .Verify(w => w.WriteSession(
                    It.IsAny<RawSession>(),
                    It.IsAny<DateTime>()));
            lifecycleHandlerBuilder
                .CookieWriter
                .Verify(w => w.WriteCookie(
                    It.IsAny<IRequest>(),
                    It.IsAny<IResponse>(),
                    It.IsAny<RawSession>(),
                    It.IsAny<DateTime>()));
        }

        [Fact]
        public void SessionIsReadOnlyAfterResponseHasBeenSent()
        {
            var session = CreateSession(new Existing(SessionId));

            var lifecycleHandler = new SessionLifecycleHandlerBuilder()
                .ConfigureSessionLoader(session)
                .ConfigureExpirationRetriever(session)
                .ConfigureSessionWriter(session)
                .ConfigureCookieWriter(session)
                .Build();

            var request = CreateMockRequest();
            var response = CreateMockResponse();

            lifecycleHandler.OnRequest(request);
            lifecycleHandler.OnResponse(request, response);

            Assert.True(session.ReadOnly);
        }

        private static RawSession ExtractRawSession(ISession session)
        {
            var sessionStub = session as SessionStub
                              ?? throw new InvalidOperationException("Unable to extract raw session");
            return sessionStub.RawSession;
        }

        private static IRequest CreateMockRequest()
        {
            var request = new Mock<IRequest>();
            return request.Object;
        }

        private static IResponse CreateMockResponse()
        {
            var request = new Mock<IResponse>();
            return request.Object;
        }


        private static RawSession CreateSession(ISessionStateVariant state)
        {
            return new RawSession(state, new SessionData(CreationDate));
        }

        private class SessionLifecycleHandlerBuilder
        {
            public MockRepository MockRepository { get; }

            public Mock<ISessionLoader> SessionLoader { get; }

            public Mock<ISessionCreator> SessionCreator { get; }

            public Mock<ISessionWriter> SessionWriter { get; }

            public Mock<ICookieWriter> CookieWriter  { get; }

            public Mock<IExpirationRetriever> ExpirationRetriever  { get; }

            public SessionLifecycleHandlerBuilder()
            {
                MockRepository = new MockRepository(MockBehavior.Strict);
                SessionLoader = MockRepository.Create<ISessionLoader>();
                SessionCreator = MockRepository.Create<ISessionCreator>();
                SessionWriter = MockRepository.Create<ISessionWriter>();
                CookieWriter = MockRepository.Create<ICookieWriter>();
                ExpirationRetriever = MockRepository.Create<IExpirationRetriever>();
            }

            public SessionLifecycleHandler Build()
            {
                return new SessionLifecycleHandler(
                    SessionLoader.Object,
                    SessionCreator.Object,
                    SessionWriter.Object,
                    (rawSession) => new SessionStub(rawSession),
                    CookieWriter.Object,
                    ExpirationRetriever.Object);
            }

            public SessionLifecycleHandlerBuilder ConfigureSessionLoader(RawSession? session)
            {
                SessionLoader
                    .Setup(l => l.LoadFromRequest(It.IsAny<IRequest>()))
                    .ReturnsAsync(session);
                return this;
            }

            public SessionLifecycleHandlerBuilder ConfigureSessionCreator(RawSession session)
            {
                SessionCreator
                    .Setup(c => c.CreateSession())
                    .Returns(session);
                return this;
            }

            public SessionLifecycleHandlerBuilder ConfigureExpirationRetriever(RawSession session)
            {
                ExpirationRetriever
                        .Setup(r => r.GetExpiration(session))
                        .Returns(IdleExpirationTime);
                return this;
            }

            public SessionLifecycleHandlerBuilder ConfigureSessionWriter(RawSession session)
            {
                SessionWriter
                    .Setup(w => w.WriteSession(session, IdleExpirationTime))
                    .Returns(Task.CompletedTask);
                return this;
            }

            public SessionLifecycleHandlerBuilder ConfigureCookieWriter(RawSession session)
            {
                CookieWriter
                    .Setup(w => w.WriteCookie(
                        It.IsAny<IRequest>(),
                        It.IsAny<IResponse>(),
                        session,
                        IdleExpirationTime));
                return this;
            }
        }

        private class SessionStub : ISession
        {
            public RawSession RawSession { get; }

            public SessionStub(RawSession rawSession)
            {
                RawSession = rawSession;
            }

            public SessionId Id => throw new NotImplementedException();

            DateTime ISession.CreationDate => throw new NotImplementedException();

            public void RenewId() => throw new NotImplementedException();

            public void Abandon() => throw new NotImplementedException();

            public void Set(string key, byte[] value) => throw new NotImplementedException();

            public byte[] Get(string key) => throw new NotImplementedException();

            public void Remove(string key) => throw new NotImplementedException();
        }
    }
}
