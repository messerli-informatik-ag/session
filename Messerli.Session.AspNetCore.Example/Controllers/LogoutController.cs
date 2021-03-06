﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace Messerli.Session.AspNetCore.Example.Controllers
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

            session.Abandon();

            return new RedirectResult("/");
        }
    }
}
