using System;
using Bash.Session.Configuration;
using Bash.Session.Http;

namespace Bash.Session.Internal
{
    internal class CookieWriter : ICookieWriter
    {
        private readonly CookieSettings _cookieSettings;

        public CookieWriter(CookieSettings cookieSettings)
        {
            _cookieSettings = cookieSettings;
        }

        public void WriteCookie(IResponse response, RawSession session, DateTime idleExpirationDate)
        {
            session.State.Map(
                mapNew: _ => WriteNew(response, session, idleExpirationDate),
                mapExisting: _ => WriteExisting(response, session, idleExpirationDate),
                mapExistingWithNewId: _ => WriteExisting(response, session, idleExpirationDate),
                mapAbandoned: _ => WriteAbandoned(response));
        }

        private void WriteNew(IResponse response, RawSession session, DateTime idleExpirationDate)
        {
            if (!session.IsEmpty())
            {
                SetCookie(response, session, idleExpirationDate);
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

        private void WriteAbandoned(IResponse response)
        {
            UnsetCookie(response);
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
