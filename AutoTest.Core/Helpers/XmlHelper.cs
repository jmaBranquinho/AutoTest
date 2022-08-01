namespace AutoTest.Core.Helpers
{
    public static class XmlHelper
    {
        public static string FormatXmlContent(this string tag, string content, params (string Name, string Value)[] parameters)
        {
            return string.IsNullOrWhiteSpace(content)
                ? $"{tag.ToTag(isClosingStatement: false, isContentless: true, parameters)}"
                : $"{tag.ToTag(isClosingStatement: false, isContentless: false, parameters)}{Environment.NewLine}{content.AddIdentation()}{Environment.NewLine}{tag.ToTag(isClosingStatement: true, isContentless: false)}";
        }

        private static string ToTag(this string tag, bool isClosingStatement = false, bool isContentless = false, params (string Name, string Value)[] parameters)
        {
            var formattedParameters = parameters.Any()
                ? $" {parameters.Select(x => $"{x.Name}=\"{x.Value}\"").JoinWithSpaces()}"
                : string.Empty;
            return $"<{(isClosingStatement ? StringExtensions.Slash : string.Empty)}{tag}{formattedParameters}{(isContentless ? $"{StringExtensions.Space}{StringExtensions.Space}" : string.Empty)}>";
        }
    }
}
