using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Helpers;
using System.Text;

namespace AutoTest.CodeGenerator.Models
{
    public class Class
    {
        private string _className;
        private IEnumerable<string> _usings;
        private IEnumerable<string> _annotations;
        private IEnumerable<ClassModifiers> _modifiers;
        private IEnumerable<(string Name, string Type, bool IsInjected)> _parameters;
        private IEnumerable<string> _methods;
        private IEnumerable<Method> _nonGeneratedMethods;

        public Class(string classname, IEnumerable<string> usings, IEnumerable<string> annotations, IEnumerable<ClassModifiers> modifiers, IEnumerable<(string Name, string Type, bool IsInjected)> parameters, IEnumerable<string> methods)
        {
            _className = classname;
            _usings = usings ?? new List<string>();
            _annotations = annotations ?? new List<string>();
            _modifiers = modifiers ?? new List<ClassModifiers>();
            _parameters = parameters ?? new List<(string Name, string Type, bool IsInjected)>();
            _methods = methods ?? new List<string> ();
            _nonGeneratedMethods = new List<Method>();
        }

        public Class(string classname, IEnumerable<string> usings, IEnumerable<string> annotations, IEnumerable<ClassModifiers> modifiers, IEnumerable<(string Name, string Type, bool IsInjected)> parameters, IEnumerable<Method> methods)
        {
            _className = classname;
            _usings = usings ?? new List<string>();
            _annotations = annotations ?? new List<string>();
            _modifiers = modifiers ?? new List<ClassModifiers>();
            _parameters = parameters ?? new List<(string Name, string Type, bool IsInjected)>();
            _nonGeneratedMethods = methods ?? new List<Method>();
            _methods = new List<string>();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var body = new List<string>();
            if (IsCtorRequired())
            {
                body.Add(AddCtor());
            }

            body.AddRange(_methods.Concat(_nonGeneratedMethods.Select(m => m.ToString())));

            stringBuilder
                .AppendJoin(Environment.NewLine, _usings)
                .Append(_usings is not null && _usings.Any() ? Environment.NewLine.Repeat(2) : string.Empty)
                .AppendJoin(Environment.NewLine, _annotations)
                .Append(_annotations is not null && _annotations.Any() ? Environment.NewLine : string.Empty)
                .Append($"{AddClassModifiers()} class {_className}");

            var parameters = AddParameters();

            return stringBuilder.ToString()
                .AddNewContext(string.Join(Environment.NewLine, parameters) +
                    (parameters.Any() ? Environment.NewLine.Repeat(2) : string.Empty) +
                    string.Join(Environment.NewLine.Repeat(2), body));
        }

        private IEnumerable<string> AddParameters()
        {
            foreach (var parameter in _parameters)
            {
                yield return $"private {parameter.Type} {parameter.Name.FormatAsPrivateField()};";
            }
        }

        private string AddClassModifiers() => string.Join(" ", _modifiers.Select(m => m.ToString().ToLowerInvariant()));

        private string AddCtor()
        {
            var dIVars = _parameters
                .Where(p => p.IsInjected)
                .ToList();

            var body = dIVars
                .Select(p => $"{p.Name.FormatAsPrivateField()} = {p.Name.FormatAsVariable()};")
                .ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"public {_className}".AddNewContext(string.Join(", ", dIVars.Select(p => $"{p.Type} {p.Name}")), Symbols.Parentheses));

            return stringBuilder
                .ToString()
                .AddNewContext(string.Join(Environment.NewLine, body));
        }

        private bool IsCtorRequired() => _parameters.Any(p => p.IsInjected);
    }
}
