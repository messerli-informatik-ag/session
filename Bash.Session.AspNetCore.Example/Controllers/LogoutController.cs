using System;
using Microsoft.AspNetCore.Mvc;

namespace Bash.Session.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("logout")]
    public class LogoutController : ControllerBase
    {
        [HttpGet]
        public RedirectResult Get()
        {
            var session = HttpContext.Features.Get<ISession>()
                ?? throw new NullReferenceException("Session was not found in context");

            const string isLoggedIn = "isLoggedIn";
            session.SetInt32(isLoggedIn, 0);
            session.Abandon();

            return new RedirectResult("/");
        }
    }
}
