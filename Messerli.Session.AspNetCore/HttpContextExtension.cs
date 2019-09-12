using Messerli.Session.AspNetCore.Internal;
using Microsoft.AspNetCore.Http;

namespace Messerli.Session.AspNetCore
{
    public static class HttpContextExtension
    {
        public static void DisableAutomaticCacheControl(this HttpContext httpContext)
        {
            httpContext
                .Features
                .Get<PerRequestSessionSettings>()
                .AutomaticCacheControl = false;
        }
    }
}
