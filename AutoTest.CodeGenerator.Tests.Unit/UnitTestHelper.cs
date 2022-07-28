using FluentAssertions;
using System;
using System.Linq;

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

        public static string GetDefaultNewLineCharAndReplaceIt(this string text)
        {
            var newLine = text[..2];
            if(newLine != "\r\n")
            {
                return text.Replace(newLine.First().ToString(), Environment.NewLine);
            }

            return text;
        }

        private static string ReOrderReformatSpaces(string text) => text
            .Replace("    ", "\t")
            .Replace($"{Environment.NewLine}\t", $"\t{Environment.NewLine}")
            .Replace($"{Environment.NewLine}\t{Environment.NewLine}", $"\t{Environment.NewLine}{Environment.NewLine}")
            .Replace($"\t{Environment.NewLine}\t", $"\t\t{Environment.NewLine}");
    }
}
