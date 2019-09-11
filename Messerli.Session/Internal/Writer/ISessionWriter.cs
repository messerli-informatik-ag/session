using System;
using System.Threading.Tasks;

namespace Messerli.Session.Internal.Writer
{
    internal interface ISessionWriter
    {
        Task WriteSession(RawSession session, DateTime idleExpirationDate);
    }
}
