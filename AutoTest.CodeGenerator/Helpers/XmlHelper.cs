namespace AutoTest.CodeGenerator.Helpers
{
    public static class XmlHelper
    {
        public static string FormatXmlContent(this string tag, string content, params (string Name, string Value)[] parameters)
        {
            if(string.IsNullOrWhiteSpace(content))
            {
                return $"{tag.ToTag(isClosingStatement: false, isContentless: true, parameters)}";
            }
            return $"{tag.ToTag(isClosingStatement: false, isContentless: false, parameters)}{Environment.NewLine}{content.AddIdentation()}{Environment.NewLine}{tag.ToTag(isClosingStatement: true, isContentless: false)}";
        }

        private static string ToTag(this string tag, bool isClosingStatement = false, bool isContentless = false, params (string Name, string Value)[] parameters)
        {
            var formattedParameters = parameters.Any() 
                ? $" {string.Join(" ", parameters.Select(x => $"{x.Name}=\"{x.Value}\""))}"
                : string.Empty;
            return $"<{(isClosingStatement ? "/" : string.Empty)}{tag}{formattedParameters}{(isContentless ? " /" : string.Empty)}>";
        }
    }
}
