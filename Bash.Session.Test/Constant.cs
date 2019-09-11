using Xunit;

namespace Bash.Session.Test
{
    internal static class Constant
    {
        public static TheoryData<string> WhitespaceValues = new TheoryData<string>
        {
            "",
            " ",
            "\t",
            "\n",
            "\r",
            "\r\n",
        };
    }
}
