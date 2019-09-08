using System;

namespace Bash.Session
{
    public class DateTimeFactory : IDateTimeFactory
    {
        public DateTime Now() => DateTime.Now;
    }
}
