using Messerli.Session.Http;

namespace Messerli.Session.Internal
{
    internal interface ICacheControlHeaderWriter
    {
        void AddCacheControlHeaders(IResponse response);
    }
}
