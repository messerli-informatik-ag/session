using System;
using Messerli.Session.Http;

namespace Messerli.Session.Internal
{
    internal class CacheControlHeaderWriter : ICacheControlHeaderWriter
    {
        private const string HeaderName = "Cache-Control";
        private const string Cacheability = "private";
        private const string Expiration = "max-age=0";
        private const string Validation = "must-revalidate";
        private static readonly string CacheControlValue = $"{Cacheability}, {Expiration}, {Validation}";

        public void AddCacheControlHeaders(IResponse response)
        {
            if (!response.AutomaticCacheControl)
            {
                return;
            }

            if (response.HasHeader(HeaderName))
            {
                throw new InvalidOperationException(
                    $"The {HeaderName} is already present in the response. If you need to set the header to a custom value, disable automatic cache control.");
            }

            response.SetHeader(HeaderName, CacheControlValue);
        }
    }
}
