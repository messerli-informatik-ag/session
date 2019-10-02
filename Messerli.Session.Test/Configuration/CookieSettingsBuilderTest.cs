using System;
using Messerli.Session.Configuration;
using Xunit;

namespace Messerli.Session.Test.Configuration
{
    public class CookieSettingsBuilderTest
    {
        [Fact]
        public void BuilderHasDefaultValues()
        {
            var unusedResult = new CookieSettingsBuilder().Build();
        }

        [Theory]
        [MemberData(nameof(InvalidCookieNames))]
        public void ThrowsIfCookieNameIsInvalid(string invalidCookieName)
        {
            var cookieName = new CookieName(invalidCookieName);
            var builder = new CookieSettingsBuilder().Name(cookieName);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var unusedResult = builder.Build();
            });
        }

        [Fact]
        public void UsesProvidedValues()
        {
            var cookieName = new CookieName("foo");
            const bool httpOnly = false;
            const CookieSecurePreference secureOnly = CookieSecurePreference.Never;
            const CookieSameSiteMode sameSiteMode = CookieSameSiteMode.Strict;
            var expectedCookieSettings = new CookieSettings(cookieName, httpOnly, secureOnly, sameSiteMode);

            var cookieSettings = new CookieSettingsBuilder()
                .Name(cookieName)
                .HttpOnly(httpOnly)
                .SecurePreference(secureOnly)
                .SameSiteMode(sameSiteMode)
                .Build();
            Assert.Equal(expectedCookieSettings, cookieSettings);
        }

        public static TheoryData<string> InvalidCookieNames()
        {
            var invalidNames = new TheoryData<string>
            {
                "foo=",
                "foo;",
                "foo\0",
                "foo👍",
                "foo\\",
                "foo(",
                "foo)",
                "foo<",
                "foo>",
                "foo@foo",
                "foo,bar",
                "foo:bar",
                "foo\"bar",
                "bar/foo",
                "foo[bar",
                "foo]bar",
                "foo?bar",
                "{foo",
                "}foo",
            };

            foreach (var value in Constant.WhitespaceValues)
            {
                invalidNames.Add(value);
            }

            return invalidNames;
        }
    }
}
