using System;
using Bash.Session.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Bash.Session.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("visit")]
    public class VisitController : ControllerBase
    {
        [HttpGet]
        public RedirectResult Get()
        {
            var session = HttpContext.Features.Get<ISession>()
                ?? throw new NullReferenceException("Session was not found in context");

            const string visitsKey = "visits";
            var visits = session.GetInt32(visitsKey) ?? 0;
            session.SetInt32(visitsKey, visits + 1);
            return new RedirectResult("/");
        }
    }
}
