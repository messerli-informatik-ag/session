using System;
using Microsoft.AspNetCore.Mvc;

namespace Bash.Session.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ContentResult  Get()
        {
            var session = HttpContext.Features.Get<ISession>()
                ?? throw new NullReferenceException("Session was not found in context");

            const string isLoggedInKey = "isLoggedIn";
            var isLoggedIn = session.GetBoolean(isLoggedInKey) ?? false;
            var visits = session.GetInt32("visits") ?? 0;

            var loginString = isLoggedIn
                ? "You are logged in"
                : "You are not logged in";

            var content = $"{loginString}<br />{visits} visits.<br/>" +
                   $"<ul>" +
                   $"<li><a href=\"/visit\">Visit</a></li>" +
                   $"<li><a href=\"/login\">Login</a></li>" +
                   $"<li><a href=\"/logout\">Logout</a></li>" +
                   $"</ul>";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = content,
            };
        }
    }
}
