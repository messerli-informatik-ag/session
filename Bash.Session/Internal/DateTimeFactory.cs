using System;

namespace Bash.Session.Internal
{
    internal class DateTimeFactory : IDateTimeFactory
    {
        public DateTime Now() => DateTime.Now;
    }
}
