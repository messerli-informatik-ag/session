using System;

namespace Messerli.Session.Internal
{
    internal class DateTimeFactory : IDateTimeFactory
    {
        public DateTime Now() => DateTime.Now;
    }
}
