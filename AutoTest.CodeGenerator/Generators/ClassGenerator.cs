using AutoTest.CodeGenerator.Helpers;
using System.Text;

namespace AutoTest.CodeGenerator.Generators
{
    // TODO: add alternatives for when usings, methods, etc. are not needed (e.g. WithNoAnnotations())
    public class ClassGenerator :
            IClassNameSelectionStage,
            IClassUsingsSelectionStage,
            IClassAnnotationSelectionStage,
            IClassMethodsSelectionStage,
            IClassGenerateSelectionStage
    {
        private string _className;
        private List<string> _usings;
        private List<string> _annotations;
        private List<(string Name, string Type, bool IsInjected)> _parameters;
        private List<string> _methods;

        private ClassGenerator()
        {
            _usings = new();
            _annotations = new();
            _parameters = new();
            _methods = new();
        }

        public static IClassNameSelectionStage NewClass() => new ClassGenerator();

        public IClassUsingsSelectionStage WithClassName(string name)
        {
            _className = name;
            return this;
        }

        public IClassUsingsSelectionStage WithUsing(string @using)
        {
            _usings.Add(@using);
            return this;
        }

        public IClassAnnotationSelectionStage WithUsings(IEnumerable<string> usings)
        {
            _usings.AddRange(@usings);
            return this;
        }

        public IClassAnnotationSelectionStage WithAnnotation(string annotation)
        {
            _annotations.Add(annotation);
            return this;
        }

        public IClassPropertiesSelectionStage WithAnnotations(IEnumerable<string> annotations)
        {
            _annotations.AddRange(annotations);
            return this;
        }

        public IClassPropertiesSelectionStage WithParameter(string parameter, string type)
        {
            _parameters.Add((parameter, type, false));
            return this;
        }

        public IClassPropertiesSelectionStage WithDIParameter(string parameter, string type)
        {
            _parameters.Add((parameter, type, true));
            return this;
        }

        public IClassMethodsSelectionStage WithMethod(string method)
        {
            _methods.Add(method);
            return this;
        }

        public string Generate()
        {
            var stringBuilder = new StringBuilder();

            var body = new List<string>();
            if (IsCtorRequired())
            {
                body.Add(AddCtor());
            }
            body.AddRange(_methods);

            stringBuilder
                .AppendJoin(Environment.NewLine, _usings)
                .Append(_usings is not null && _usings.Count > 0 ? Environment.NewLine.Repeat(2) : string.Empty)
                .AppendJoin(Environment.NewLine, _annotations)
                .Append(_annotations is not null && _annotations.Count > 0 ? Environment.NewLine : string.Empty)
                .Append($"public class {_className}");

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

        private string AddCtor()
        {
            var dIVars = _parameters
                .Where(p => p.IsInjected)
                .ToList();

            var body = dIVars
                .Select(p => $"{p.Name.FormatAsPrivateField()} = {p.Name.FormatAsVariable()};")
                .ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"public {_className}(");
            stringBuilder.AppendJoin(", ", dIVars.Select(p => $"{p.Type} {p.Name}"));
            stringBuilder.Append(')');

            return stringBuilder
                .ToString()
                .AddNewContext(string.Join(Environment.NewLine, body));
        }

        // TODO: missing some more scenarios
        private bool IsCtorRequired() => _parameters.Any(p => p.IsInjected);
    }

    public interface IClassNameSelectionStage
    {
        public IClassUsingsSelectionStage WithClassName(string name);
    }

    public interface IClassUsingsSelectionStage : IClassAnnotationSelectionStage
    {
        public IClassUsingsSelectionStage WithUsing(string @using);

        public IClassAnnotationSelectionStage WithUsings(IEnumerable<string> usings);
    }

    public interface IClassGenerateSelectionStage
    {
        public string Generate();
    }

    public interface IClassMethodsSelectionStage : IClassGenerateSelectionStage
    {
        public IClassMethodsSelectionStage WithMethod(string method);
    }

    public interface IClassPropertiesSelectionStage : IClassMethodsSelectionStage
    {
        public IClassPropertiesSelectionStage WithParameter(string parameter, string type);

        public IClassPropertiesSelectionStage WithDIParameter(string parameter, string type);
    }

    public interface IClassAnnotationSelectionStage : IClassPropertiesSelectionStage
    {
        public IClassAnnotationSelectionStage WithAnnotation(string annotation);

        public IClassPropertiesSelectionStage WithAnnotations(IEnumerable<string> annotations);
    }
}
