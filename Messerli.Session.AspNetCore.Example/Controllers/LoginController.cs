﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace Messerli.Session.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public RedirectResult Get()
        {
            var session = HttpContext.Features.Get<ISession>()
                ?? throw new NullReferenceException("Session was not found in context");

            const string isLoggedIn = "isLoggedIn";
            session.SetBoolean(isLoggedIn, true);
            session.RenewId();

            return new RedirectResult("/");
        }
    }
}
