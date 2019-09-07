using System;

namespace Bash.AspNetCore.Session
{
    public interface IDateTimeFactory
    {
        DateTime Now();
    }
}
