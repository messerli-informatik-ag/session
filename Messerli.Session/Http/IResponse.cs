namespace Messerli.Session.Http
{
    public interface IResponse
    {
        bool AutomaticCacheControl { get; }

        void SetCookie(Cookie cookie);

        void SetHeader(string name, string value);

        bool HasHeader(string name);
    }
}
