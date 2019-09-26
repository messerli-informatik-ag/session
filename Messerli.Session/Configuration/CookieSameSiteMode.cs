namespace Messerli.Session.Configuration
{
    /// <summary>
    /// Corresponds with the <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie">SameSite</a> directive
    /// on cookies.
    /// </summary>
    public enum CookieSameSiteMode
    {
        None,
        Lax,
        Strict,
    }
}
