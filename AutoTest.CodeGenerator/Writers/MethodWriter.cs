using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Helpers;
using AutoTest.CodeGenerator.Models;
using System.Text;

namespace AutoTest.CodeGenerator.Writers
{
    public class MethodWriter
    {
        public static string GetText(Method method) 
            => new StringBuilder()
                .AppendJoin(Environment.NewLine, method.Annotations)
                .Append(method.Annotations.Any() ? Environment.NewLine : string.Empty)
                .Append($"{GenerateMethodModifiers(method.Modifiers)} {method.ReturnType} {method.Name}")
                .ToString()
                .AddNewContext(string.Join(", ", AddParameters(method.Parameters)), Symbols.Parentheses)
                .AddNewContext(method.Body);

        private static string GenerateMethodModifiers(IEnumerable<MethodModifiers> modifiers) 
            => string.Join(" ", modifiers.Select(m => m.ToString().ToLowerInvariant()));

        private static IEnumerable<string> AddParameters(IEnumerable<(string Name, string Type)> parameters)
        {
            foreach (var parameter in parameters)
            {
                yield return $"{parameter.Type} {parameter.Name.FormatAsVariable()}";
            }
        }
    }
}
