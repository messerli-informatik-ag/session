namespace Messerli.Session.Http
{
    public interface IResponse
    {
        bool AutomaticCacheControl { get; }

        void SetCookie(Cookie cookie);

        void SetHeader(string name, string value);

        string? GetFirstHeaderValue(string name);
    }
}
