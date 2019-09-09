using System;
using Microsoft.AspNetCore.Mvc;

namespace Bash.Session.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var session = HttpContext.Features.Get<ISession>()
                ?? throw new NullReferenceException("Session was not found in context");

            const string isLoggedInKey = "isLoggedIn";
            var isLoggedIn = session.GetInt32(isLoggedInKey);

            return isLoggedIn == 1
                ? "You are logged in"
                : "You are not logged in";
        }
    }
}
