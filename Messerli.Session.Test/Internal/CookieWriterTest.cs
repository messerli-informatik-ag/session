using System;
using System.Collections.Generic;
using Messerli.Session.Configuration;
using Messerli.Session.Http;
using Messerli.Session.Internal;
using Messerli.Session.SessionState;
using Messerli.Session.Storage;
using Moq;
using Xunit;

namespace Messerli.Session.Test.Internal
{
    public class CookieWriterTest
    {
        private static readonly CookieName CookieName = new CookieName("session_id");

        private static readonly SessionId SessionId = new SessionId("foo-bar-baz");

        private static readonly DateTime CreationDate = DateTime.UnixEpoch;

        private static readonly SessionData EmptySessionData = new SessionData(CreationDate);

        private static readonly SessionData NonEmptySessionData =
            new SessionData(
                CreationDate,
                new Dictionary<string, byte[]>
                {
                    { "foo", new byte[0] },
                });

        private static readonly DateTime Expiration = new DateTime(
            year: 2010,
            month: 10,
            day: 10,
            hour: 10,
            minute: 10,
            second: 10);

        private static readonly CookieSettings CookieSettings = new CookieSettingsBuilder()
            .Name(CookieName)
            .Build();

        private static readonly Cookie Cookie = new Cookie(CookieSettings, SessionId.Value, Expiration);

        private static readonly Cookie UnsetCookie = new Cookie(CookieSettings, string.Empty, DateTime.UnixEpoch);

        [Theory]
        [MemberData(nameof(TestData))]
        public void TestCookieIsWrittenAndRemovedCorrectly(WriteCookieTestParameters parameters)
        {
            var response = new Mock<IResponse>();
            var request = MockRequest(parameters);
            var sessionData = parameters.HasSessionData ? NonEmptySessionData : EmptySessionData;
            var expectedCookie = ExpectedCookie(parameters);

            var cookieWriter = new CookieWriter(CookieSettings);
            cookieWriter.WriteCookie(
                request.Object,
                response.Object,
                new RawSession(parameters.State, sessionData),
                Expiration);

            if (expectedCookie is { } cookie)
            {
                response.Verify(r => r.SetCookie(cookie));
                response.Verify(r => r.SetHeader(It.IsAny<string>(), It.IsAny<string>()));
            }

            response.VerifyNoOtherCalls();
        }

        private static Mock<IRequest> MockRequest(WriteCookieTestParameters parameters)
        {
            var request = new Mock<IRequest>();
            request
                .Setup(r => r.HasCookie(CookieName))
                .Returns(parameters.CookieWasSetInRequest);
            return request;
        }

        private static Cookie? ExpectedCookie(WriteCookieTestParameters parameters)
        {
            return parameters.ExpectedAction switch
            {
                Action.SetCookie => Cookie,
                Action.RemoveCookie => UnsetCookie,
                _ => null,
            };
        }

        public static TheoryData<object> TestData()
        {
            return new TheoryData<object>
            {
                new WriteCookieTestParameters(
                    state: new New(SessionId),
                    cookieWasSetInRequest: false,
                    hasSessionData: false,
                    expectedAction: Action.None),
                new WriteCookieTestParameters(
                    state: new New(SessionId),
                    cookieWasSetInRequest: false,
                    hasSessionData: true,
                    expectedAction: Action.SetCookie),
                new WriteCookieTestParameters(
                    state: new New(SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: false,
                    expectedAction: Action.RemoveCookie),
                new WriteCookieTestParameters(
                    state: new New(SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: true,
                    expectedAction: Action.SetCookie),
                new WriteCookieTestParameters(
                    state: new Existing(SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: false,
                    expectedAction: Action.RemoveCookie),
                new WriteCookieTestParameters(
                    state: new Existing(SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: true,
                    expectedAction: Action.SetCookie),
                new WriteCookieTestParameters(
                    state: new ExistingWithNewId(SessionId, SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: false,
                    expectedAction: Action.RemoveCookie),
                new WriteCookieTestParameters(
                    state: new ExistingWithNewId(SessionId, SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: true,
                    expectedAction: Action.SetCookie),
                new WriteCookieTestParameters(
                    state: new Abandoned(SessionId),
                    cookieWasSetInRequest: false,
                    hasSessionData: false,
                    expectedAction: Action.None),
                new WriteCookieTestParameters(
                    state: new Abandoned(SessionId),
                    cookieWasSetInRequest: false,
                    hasSessionData: true,
                    expectedAction: Action.None),
                new WriteCookieTestParameters(
                    state: new Abandoned(SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: false,
                    expectedAction: Action.RemoveCookie),
                new WriteCookieTestParameters(
                    state: new Abandoned(SessionId),
                    cookieWasSetInRequest: true,
                    hasSessionData: true,
                    expectedAction: Action.RemoveCookie),
            };
        }

        public enum Action
        {
            None,
            SetCookie,
            RemoveCookie,
        }

        public class WriteCookieTestParameters
        {
            internal ISessionStateVariant State { get; set; }

            internal bool CookieWasSetInRequest { get; set; }

            internal bool HasSessionData { get; set; }

            internal Action ExpectedAction { get; set; }

            internal WriteCookieTestParameters(
                ISessionStateVariant state,
                bool cookieWasSetInRequest,
                bool hasSessionData,
                Action expectedAction)
            {
                State = state;
                CookieWasSetInRequest = cookieWasSetInRequest;
                HasSessionData = hasSessionData;
                ExpectedAction = expectedAction;
            }

            public override string ToString()
            {
                return $"{State}, " +
                       $"{nameof(CookieWasSetInRequest)} = {CookieWasSetInRequest}, " +
                       $"{nameof(HasSessionData)} = {HasSessionData}, " +
                       $"{nameof(ExpectedAction)} = {ExpectedAction}";
            }
        }
    }
}
