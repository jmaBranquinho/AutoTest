using FluentAssertions;

namespace AutoTest.CodeGenerator.Tests.Unit
{
    public static class UnitTestHelper
    {
        public static void AssertSimilarStrings(string expected, string actual)
        {
            expected = ReOrderReformatSpaces(expected);
            actual = ReOrderReformatSpaces(actual);

            actual.Should().BeEquivalentTo(expected);
        }

        private static string ReOrderReformatSpaces(string text) => text
            .Replace("    ", "\t")
            .Replace("\r\n\t", "\t\r\n")
            .Replace("\r\n\t\r\n", "\t\r\n\r\n")
            .Replace("\t\r\n\t", "\t\t\r\n");
    }
}
