using System;
using System.Threading.Tasks;
using Bash.Session.Http;
using Bash.Session.Internal;
using Bash.Session.Internal.Writer;
using Bash.Session.SessionState;
using Bash.Session.Storage;
using Moq;
using Xunit;

namespace Bash.Session.Test.Internal
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
        public void SessionIsWrittenToStorageAndCookie()
        {
            var session = CreateSession(new Existing(SessionId));

            var lifecycleHandler = new SessionLifecycleHandlerBuilder()
                .ConfigureSessionLoader(session)
                .ConfigureIdleExpirationRetriever()
                .ConfigureSessionWriter(session)
                .ConfigureCookieWriter(session)
                .Build();

            var request = CreateMockRequest();
            var response = CreateMockResponse();

            lifecycleHandler.OnRequest(request);
            lifecycleHandler.OnResponse(request, response);
        }

        [Fact]
        public void SessionIsReadOnlyAfterResponseHasBeenSent()
        {
            var session = CreateSession(new Existing(SessionId));

            var lifecycleHandler = new SessionLifecycleHandlerBuilder()
                .ConfigureSessionLoader(session)
                .ConfigureIdleExpirationRetriever()
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

            public Mock<IIdleExpirationRetriever> IdleExpirationRetriever  { get; }

            public SessionLifecycleHandlerBuilder()
            {
                MockRepository = new MockRepository(MockBehavior.Strict);
                SessionLoader = MockRepository.Create<ISessionLoader>();
                SessionCreator = MockRepository.Create<ISessionCreator>();
                SessionWriter = MockRepository.Create<ISessionWriter>();
                CookieWriter = MockRepository.Create<ICookieWriter>();
                IdleExpirationRetriever = MockRepository.Create<IIdleExpirationRetriever>();
            }

            public SessionLifecycleHandler Build()
            {
                return new SessionLifecycleHandler(
                    SessionLoader.Object,
                    SessionCreator.Object,
                    SessionWriter.Object,
                    (rawSession) => new SessionStub(rawSession),
                    CookieWriter.Object,
                    IdleExpirationRetriever.Object);
            }

            public SessionLifecycleHandlerBuilder ConfigureSessionLoader(RawSession? session)
            {
                SessionLoader
                    .Setup(l => l.LoadFromRequest(It.IsAny<IRequest>()))
                    .Returns(Task.FromResult(session));
                return this;
            }

            public SessionLifecycleHandlerBuilder ConfigureSessionCreator(RawSession session)
            {
                SessionCreator
                    .Setup(c => c.CreateSession())
                    .Returns(session);
                return this;
            }

            public SessionLifecycleHandlerBuilder ConfigureIdleExpirationRetriever()
            {
                IdleExpirationRetriever
                    .Setup(r => r.GetIdleExpiration())
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

            public SessionId Id { get; }

            public DateTime CreationDate { get; }

            public void RenewId()
            {
                throw new NotImplementedException();
            }

            public void Abandon()
            {
                throw new NotImplementedException();
            }

            public void Set(string key, byte[] value)
            {
                throw new NotImplementedException();
            }

            public byte[] Get(string key)
            {
                throw new NotImplementedException();
            }

            public void Remove(string key)
            {
                throw new NotImplementedException();
            }
        }
    }
}
