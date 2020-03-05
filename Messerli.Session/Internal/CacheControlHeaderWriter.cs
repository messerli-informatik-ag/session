using System;
using System.Net.Http.Headers;
using Messerli.Session.Http;

namespace Messerli.Session.Internal
{
    internal class CacheControlHeaderWriter : ICacheControlHeaderWriter
    {
        private const string HeaderName = "Cache-Control";

        public void AddCacheControlHeaders(IResponse response)
        {
            if (!response.AutomaticCacheControl)
            {
                return;
            }

            if (response.GetFirstHeaderValue(HeaderName) is { } headerValue)
            {
                ValidateExistingHeader(headerValue);
            }
            else
            {
                SetCacheControlHeader(response);
            }
        }

        private static void ValidateExistingHeader(string headerValue)
        {
            var cacheControlHeader = CacheControlHeaderValue.Parse(headerValue);

            if (!cacheControlHeader.NoCache)
            {
                throw new InvalidOperationException(
                    $"The {HeaderName} header is already present in the response and does not disable caching completely. " +
                    $"If you need to set the header to a custom value, disable automatic cache control.");
            }
        }

        private static void SetCacheControlHeader(IResponse response)
            => response.SetHeader(HeaderName, CreateCacheControlHeader().ToString());

        private static CacheControlHeaderValue CreateCacheControlHeader()
            => new CacheControlHeaderValue
            {
                Private = true,
                MaxAge = TimeSpan.Zero,
                MustRevalidate = true,
            };
    }
}
