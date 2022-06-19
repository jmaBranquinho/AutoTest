using System.Text;

namespace AutoTest.CodeGenerator.Helpers
{
    public static class StringExtensions
    {
        public static string FormatAsPrivateField(this string text) => $"_{ChangeFirstLetterCase(text, isToBeSetToUpper: false)}";

        public static string FormatAsPublicField(this string text) => ChangeFirstLetterCase(text, isToBeSetToUpper: true);

        public static string FormatAsVariable(this string text) => ChangeFirstLetterCase(text, isToBeSetToUpper: false);

        public static string AddNewContext(this string prefix, string content)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{prefix}{Environment.NewLine}{{{Environment.NewLine}");
            stringBuilder.Append(AddIdentation(content));
            stringBuilder.Append($"{Environment.NewLine}}}");

            return stringBuilder.ToString();
        }

        public static IEnumerable<string> AddIdentation(this IEnumerable<string> text, int identationLevel = 1) => text.Select(t => t.AddIdentation(identationLevel));

        public static string AddIdentation(this string text, int identationLevel = 1)
        {
            var identation = "\t";
            return $"{Repeat(identation, identationLevel)}{text.Replace(Environment.NewLine, $"{Environment.NewLine}{Repeat(identation, identationLevel)}")}";
        }

        public static string Repeat(this string text, int times) => new StringBuilder().Insert(0, text, times).ToString();

        private static string ChangeFirstLetterCase(string text, bool isToBeSetToUpper)
        {
            Func<char, string> changeCase = isToBeSetToUpper ? input => input.ToString().ToUpper() : input => input.ToString().ToLower();
            return $"{changeCase(text[0])}{text[1..]}";
        }
    }
}
