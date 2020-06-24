using System;
using Messerli.Session.Configuration;
using Messerli.Session.Http;

namespace Messerli.Session.Internal
{
    internal class CookieWriter : ICookieWriter
    {
        private readonly CookieSettings _cookieSettings;

        private readonly ICacheControlHeaderWriter _cacheControlHeaderWriter;

        public CookieWriter(
            CookieSettings cookieSettings,
            ICacheControlHeaderWriter cacheControlHeaderWriter)
        {
            _cookieSettings = cookieSettings;
            _cacheControlHeaderWriter = cacheControlHeaderWriter;
        }

        public void WriteCookie(IRequest request, IResponse response, RawSession session, DateTime expirationDate)
        {
            session.State.Match(
                @new: _ => WriteNew(request, response, session, expirationDate),
                existing: _ => WriteExisting(response, session, expirationDate),
                existingWithNewId: _ => WriteExisting(response, session, expirationDate),
                abandoned: _ => WriteAbandoned(request, response));
        }

        private void WriteNew(IRequest request, IResponse response, RawSession session, DateTime expirationDate)
        {
            var isSessionEmpty = session.IsEmpty();

            if (!isSessionEmpty)
            {
                SetCookie(response, session, expirationDate);
            }
            else if (RequestHasSessionIdCookie(request))
            {
                UnsetCookie(response);
            }
        }

        private void WriteExisting(IResponse response, RawSession session, DateTime expirationDate)
        {
            if (session.IsEmpty())
            {
                UnsetCookie(response);
            }
            else
            {
                SetCookie(response, session, expirationDate);
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
            _cacheControlHeaderWriter.AddCacheControlHeaders(response);
        }

        private void SetCookie(IResponse response, RawSession session, DateTime expirationDate)
        {
            response.SetCookie(
                new Cookie(
                    _cookieSettings,
                    session.GetId().ToString(),
                    expirationDate));
            _cacheControlHeaderWriter.AddCacheControlHeaders(response);
        }
    }
}
