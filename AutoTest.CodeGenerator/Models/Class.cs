using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Helpers;
using System.Text;

namespace AutoTest.CodeGenerator.Models
{
    public class Class
    {
        public string ClassName { get; set; }
        public IEnumerable<string> Imports { get; set; }
        public string Namespace { get; set; }
        public IEnumerable<string> Annotations { get; set; }
        public IEnumerable<ClassModifiers> Modifiers { get; set; }
        public IEnumerable<(string Name, string Type, bool IsInjected)> Parameters { get; set; }
        public IEnumerable<Method> Methods { get; set; }

        public Class(
            string classname, 
            string @namespace, 
            IEnumerable<string> usings, 
            IEnumerable<string> annotations, 
            IEnumerable<ClassModifiers> modifiers, 
            IEnumerable<(string Name, string Type, bool IsInjected)> parameters, 
            IEnumerable<Method> methods)
        {
            ClassName = classname;
            Imports = usings ?? new List<string>();
            Namespace = @namespace;
            Annotations = annotations ?? new List<string>();
            Modifiers = modifiers ?? new List<ClassModifiers>();
            Parameters = parameters ?? new List<(string Name, string Type, bool IsInjected)>();
            Methods = methods ?? new List<Method>();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var body = new List<string>();
            if (IsCtorRequired())
            {
                body.Add(AddCtor());
            }

            body.AddRange(Methods.Select(m => m.ToString()));

            stringBuilder
                .AppendJoin(Environment.NewLine, Imports)
                .Append(Imports is not null && Imports.Any() ? Environment.NewLine.Repeat(2) : string.Empty)
                .AppendJoin(Environment.NewLine, Annotations)
                .Append(Annotations is not null && Annotations.Any() ? Environment.NewLine : string.Empty)
                .Append($"{AddClassModifiers()} class {ClassName}");

            var parameters = AddParameters();

            return stringBuilder.ToString()
                .AddNewContext(string.Join(Environment.NewLine, parameters) +
                    (parameters.Any() ? Environment.NewLine.Repeat(2) : string.Empty) +
                    string.Join(Environment.NewLine.Repeat(2), body));
        }

        public void ToFile(string path)
        {
            var attributes = File.GetAttributes(path);
            if(attributes.HasFlag(FileAttributes.Directory))
            {
                if(Directory.Exists(path))
                {
                    path += $"\\{ClassName}.cs";
                }
                else
                {
                    throw new Exception("Directory does not exist!");// TODO
                }
            } 
            else
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
            }
            File.WriteAllText(path, ToString());
        }

        private IEnumerable<string> AddParameters()
        {
            foreach (var parameter in Parameters)
            {
                yield return $"private {parameter.Type} {parameter.Name.FormatAsPrivateField()};";
            }
        }

        private string AddClassModifiers() => string.Join(" ", Modifiers.Select(m => m.ToString().ToLowerInvariant()));

        private string AddCtor()
        {
            var dIVars = Parameters
                .Where(p => p.IsInjected)
                .ToList();

            var body = dIVars
                .Select(p => $"{p.Name.FormatAsPrivateField()} = {p.Name.FormatAsVariable()};")
                .ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"public {ClassName}".AddNewContext(string.Join(", ", dIVars.Select(p => $"{p.Type} {p.Name}")), Symbols.Parentheses));

            return stringBuilder
                .ToString()
                .AddNewContext(string.Join(Environment.NewLine, body));
        }

        private bool IsCtorRequired() => Parameters.Any(p => p.IsInjected);
    }
}
