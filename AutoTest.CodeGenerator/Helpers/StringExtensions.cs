using AutoTest.CodeGenerator.Enums;
using System.Text;

namespace AutoTest.CodeGenerator.Helpers
{
    public static class StringExtensions
    {
        public const string Space = " ";
        public const string Identation = "\t";
        public const string Underscore = "_";
        public const string Comma = ",";

        public static string FormatAsPrivateField(this string text) => $"{Underscore}{ChangeFirstLetterCase(text, isToBeSetToUpper: false)}";

        public static string FormatAsPublicField(this string text) => ChangeFirstLetterCase(text, isToBeSetToUpper: true);

        public static string FormatAsVariable(this string text) => ChangeFirstLetterCase(text, isToBeSetToUpper: false);

        public static string AddNewContext(this string prefix, string content, Symbols symbol = Symbols.Braces)
        {
            var (openContext, closeContext, isSpaceRequired) = GetContextSymbols(symbol);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{prefix}{(isSpaceRequired ? Environment.NewLine : string.Empty)}{openContext}{(isSpaceRequired ? Environment.NewLine : string.Empty)}");
            stringBuilder.Append(isSpaceRequired ? AddIdentation(content) : content);
            stringBuilder.Append($"{(isSpaceRequired ? Environment.NewLine : string.Empty)}{closeContext}");

            return stringBuilder.ToString();
        }

        public static string EndStatement(this string text, bool hasLineBreak = false) => $"{text};{(hasLineBreak ? Environment.NewLine : string.Empty)}";

        public static IEnumerable<string> AddIdentation(this IEnumerable<string> text, int identationLevel = 1) => text.Select(t => t.AddIdentation(identationLevel));

        public static string AddIdentation(this string text, int identationLevel = 1) 
            => $"{Repeat(Identation, identationLevel)}{text.Replace(Environment.NewLine, $"{Environment.NewLine}{Repeat(Identation, identationLevel)}")}";

        public static string Repeat(this string text, int times) => new StringBuilder().Insert(0, text, times).ToString();

        public static string JoinWithSpaces<T>(this IEnumerable<T> text) => text.JoinWithString(Space);

        public static string JoinWithComma<T>(this IEnumerable<T> text, bool AddSpaces = true) => text.JoinWithString(Comma, AddSpaces ? Space : string.Empty);

        public static string JoinWithString<T>(this IEnumerable<T> text, params string[] separators) 
            => string.Join(string.Join(string.Empty, separators), text);

        private static string ChangeFirstLetterCase(string text, bool isToBeSetToUpper)
        {
            Func<char, string> changeCase = isToBeSetToUpper ? input => input.ToString().ToUpper() : input => input.ToString().ToLower();
            return $"{changeCase(text[0])}{text[1..]}";
        }

        private static (char openContext, char closeContext, bool isSpaceRequired) GetContextSymbols(Symbols symbol) 
            => symbol switch
            {
                Symbols.Brackets => ('[', ']', false),
                Symbols.Parentheses => ('(', ')', false),
                _ => ('{', '}', true),
            };
    }
}
