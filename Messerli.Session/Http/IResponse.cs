namespace Messerli.Session.Http
{
    public interface IResponse
    {
        void SetCookie(Cookie cookie);

        void SetHeader(string name, string value);
    }
}
