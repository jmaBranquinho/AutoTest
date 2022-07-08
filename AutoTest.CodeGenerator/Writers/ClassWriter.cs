using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Helpers;
using AutoTest.CodeGenerator.Models;
using System.Text;

namespace AutoTest.CodeGenerator.Writers
{
    public class ClassWriter
    {
        public static void WriteToFile(Class @class, string path)
        {
            if(!File.Exists(path))
            {
                File.Create(path);
            }

            File.WriteAllText(path, GetText(@class));
        }

        public static string GetText(Class @class)
        {
            var stringBuilder = new StringBuilder();
            var methods = GenerateMethods(@class);
            var parameters = GenerateParameters(@class).ToList();

            var formattedParameters = string.Join(Environment.NewLine, parameters);
            var formattedMethods = string.Join(Environment.NewLine.Repeat(2), @class.Methods);

            return stringBuilder
                .AppendJoin(Environment.NewLine, @class.Imports)
                .Append(@class.Imports is not null && @class.Imports.Any() ? Environment.NewLine.Repeat(2) : string.Empty)
                .AppendJoin(Environment.NewLine, @class.Annotations)
                .Append(@class.Annotations is not null && @class.Annotations.Any() ? Environment.NewLine : string.Empty)
                .Append($"{GenerateModifiers(@class)} class {@class.Name}")
                .ToString()
                .AddNewContext($"{formattedParameters}{(parameters.Any() ? Environment.NewLine.Repeat(2) : string.Empty)}{formattedMethods}");
        }

        private static string GenerateConstructor(Class @class)
        {
            var dIVars = @class.Parameters
                .Where(p => p.IsInjected)
                .ToList();

            var body = dIVars
                .Select(p => $"{p.Name.FormatAsPrivateField()} = {p.Name.FormatAsVariable()};")
                .ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"public {@class.Name}".AddNewContext(string.Join(", ", dIVars.Select(p => $"{p.Type} {p.Name}")), Symbols.Parentheses));

            return stringBuilder
                .ToString()
                .AddNewContext(string.Join(Environment.NewLine, body));
        }

        private static IEnumerable<string> GenerateMethods(Class @class)
        {
            var methods = new List<string>(); 
            var isCtorRequired = @class.Parameters.Any(x => x.IsInjected);
            if (isCtorRequired)
            {
                methods.Add(GenerateConstructor(@class));
            }
            methods.AddRange(@class.Methods.Select(x => x.ToString()));

            return methods;
        }

        private static string GenerateModifiers(Class @class) => string.Join(" ", @class.Modifiers.Select(m => m.ToString().ToLowerInvariant()));

        private static IEnumerable<string> GenerateParameters(Class @class)
        {
            foreach (var parameter in @class.Parameters)
            {
                yield return $"private {parameter.Type} {parameter.Name.FormatAsPrivateField()};";
            }
        }
    }
}
