using System.Threading.Tasks;

namespace Bash.Session.Infrastructure.Writer
{
    public class SessionWriter : ISessionWriter
    {
        public delegate IOneTimeSessionWriter CreateOneTimeSessionWriter(InternalSession session);

        private readonly CreateOneTimeSessionWriter _createOneTimeSessionWriter;

        public SessionWriter(CreateOneTimeSessionWriter createOneTimeSessionWriter)
        {
            _createOneTimeSessionWriter = createOneTimeSessionWriter;
        }

        public async Task WriteSession(InternalSession session)
        {
            var oneTimeSessionWriter = _createOneTimeSessionWriter(session);
            await session.State.Accept(oneTimeSessionWriter);
        }
    }
}
