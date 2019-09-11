namespace Messerli.Session.Configuration
{
    /// <summary>
    /// Corresponds with the <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie">Secure</a> directive
    /// on cookies.
    /// </summary>
    public enum CookieSecurePreference
    {
        /// <summary>
        /// The Secure directive will never be set. This is not recommended.
        /// </summary>
        Never,
        /// <summary>
        /// The Secure directive on the cookie will always be set.
        /// </summary>
        Always,
        /// <summary>
        /// The Secure directive will be set, if the request was sent over HTTPS. This is the default option.
        /// </summary>
        MatchingRequest,
    }
}
