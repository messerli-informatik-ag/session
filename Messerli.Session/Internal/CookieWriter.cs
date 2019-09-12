using System;
using Messerli.Session.Configuration;
using Messerli.Session.Http;

namespace Messerli.Session.Internal
{
    internal class CookieWriter : ICookieWriter
    {
        private readonly CookieSettings _cookieSettings;

        public CookieWriter(CookieSettings cookieSettings)
        {
            _cookieSettings = cookieSettings;
        }

        public void WriteCookie(IRequest request, IResponse response, RawSession session, DateTime idleExpirationDate)
        {
            session.State.Map(
                mapNew: _ => WriteNew(request, response, session, idleExpirationDate),
                mapExisting: _ => WriteExisting(response, session, idleExpirationDate),
                mapExistingWithNewId: _ => WriteExisting(response, session, idleExpirationDate),
                mapAbandoned: _ => WriteAbandoned(request, response));
        }

        private void WriteNew(IRequest request, IResponse response, RawSession session, DateTime idleExpirationDate)
        {
            var isSessionEmpty = session.IsEmpty();

            if (!isSessionEmpty)
            {
                SetCookie(response, session, idleExpirationDate);
            }
            else if (RequestHasSessionIdCookie(request))
            {
                UnsetCookie(response);
            }
        }

        private void WriteExisting(IResponse response, RawSession session, DateTime idleExpirationDate)
        {
            if (session.IsEmpty())
            {
                UnsetCookie(response);
            }
            else
            {
                SetCookie(response, session, idleExpirationDate);
            }
        }

        private void WriteAbandoned(IRequest request, IResponse response)
        {
            if (RequestHasSessionIdCookie(request))
            {
                UnsetCookie(response);
            }
        }

        private bool RequestHasSessionIdCookie(IRequest request)
        {
            return request.HasCookie(_cookieSettings.Name);
        }

        private void UnsetCookie(IResponse response)
        {
            response.SetCookie(
                new Cookie(
                    _cookieSettings,
                    string.Empty,
                    DateTime.UnixEpoch));
            SetCacheControlHeader(response);
        }

        private void SetCookie(IResponse response, RawSession session, DateTime idleExpirationDate)
        {
            response.SetCookie(
                new Cookie(
                    _cookieSettings,
                    session.GetId().ToString(),
                    idleExpirationDate));
            SetCacheControlHeader(response);
        }

        private static void SetCacheControlHeader(IResponse response)
        {
            // TODO: make this configurable
            const string cacheControlHeader = "Cache-Control";
            response.SetHeader(cacheControlHeader, "private");
        }
    }
}
