# Changelog

## 0.1.0
- Initial release

## 0.1.1
- Add missing metadata to NuGet packages.

## 0.1.2
- Fix incorrect repository URL in NuGet package metadata.

## 0.2.0
- Depend on latest version (3.1.x) of Microsoft.Extensions.Caching.Abstractions.
- Depend on latest version (3.1.x) of Microsoft.Extensions.Logging.Abstractions.

## 0.3.0
- Do not throw when `Cache-Control` is already present and set to `no-cache`.
  This was an issue when using the session middleware and ASP.NET Core's built-in anti forgery handling.
