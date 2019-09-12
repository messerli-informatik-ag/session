using System.Linq;
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

        public void AddCacheControlHeaders(IResponse response) => response.SetHeader(HeaderName, CacheControlValue);
    }
}
