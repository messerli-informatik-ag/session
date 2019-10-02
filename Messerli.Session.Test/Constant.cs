using System.Collections.Generic;
using Xunit;

namespace Messerli.Session.Test
{
    internal static class Constant
    {
        public static IEnumerable<string> WhitespaceValues = new[]
        {
            "",
            " ",
            "\t",
            "\n",
            "\r",
            "\r\n",
        };

        public static TheoryData<string> WhitespaceValuesAsTheoryData = EnumerableToTheoryData(WhitespaceValues);

        private static TheoryData<T> EnumerableToTheoryData<T>(IEnumerable<T> values)
        {
            var theoryData = new TheoryData<T>();

            foreach (var value in values)
            {
                theoryData.Add(value);
            }

            return theoryData;
        }
    }
}
