using System;

namespace Bash.Session
{
    internal class DateTimeFactory : IDateTimeFactory
    {
        public DateTime Now() => DateTime.Now;
    }
}
