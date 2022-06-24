using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Wrappers;
using System.Text;

namespace AutoTest.TestGenerator.Generators.XUnit.Models
{
    public class XUnitTest : Method
    {
        private const string _parameterlessMethodAnnotation = "[Fact]";
        private const string _parameterMethodAnnotation = "[Theory]";
        private const string _parameterAnnotationTemplate = "[InlineData({0})]";

        private IEnumerable<StatementWrapper> _methodStatements;

        public XUnitTest(string name, IEnumerable<StatementWrapper> methodStatements)
            : base(name, new List<string>() { _parameterlessMethodAnnotation }, Enumerable.Empty<(string Name, string Type)>(), FormatXUnitTestBody(methodStatements)) 
        {
            // TODO validate if should be parameterized test
        }

        public XUnitTest(string name, IEnumerable<IEnumerable<(string Name, string Type, object Value)>> parameters, IEnumerable<StatementWrapper> methodStatements) 
            : base(name, FormatXUnitParameterTestAnnotations(parameters), FormatXUnitTestMethodParameter(parameters), FormatXUnitTestBody(methodStatements)) { }

        private static string FormatXUnitTestBody(IEnumerable<StatementWrapper> methodStatements)
        {
            return string.Join(Environment.NewLine, GenerateArrangeSection(), GenerateActSection(), GenerateAssertSection());
        }

        private static string GenerateArrangeSection()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// Arrange");
            // TODO: implement

            return stringBuilder.ToString();
        }

        private static string GenerateActSection()
        {
            // TODO: implement

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// Act");
            //var methodDeclaration = (MethodDeclarationSyntax) _methodStatements.First().SyntaxNode;
            //stringBuilder.AppendLine($"var result = _sut.{methodDeclaration}();");

            return stringBuilder.ToString();
        }

        private static string GenerateAssertSection()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// Assert");
            // TODO: implement

            return stringBuilder.ToString();
        }

        private static IEnumerable<(string Name, string Type)> FormatXUnitTestMethodParameter(IEnumerable<IEnumerable<(string Name, string Type, object Value)>> parametersList)
        {
            if (parametersList is null || !parametersList.Any())
            {
                return Enumerable.Empty<(string Name, string Type)>();
            }

            return parametersList.First().Select(p => (p.Name, p.Type));
        }

        private static IEnumerable<string> FormatXUnitParameterTestAnnotations(IEnumerable<IEnumerable<(string Name, string Type, object Value)>> parametersList)
        {
            if (parametersList is null || !parametersList.Any())
            {
                return new List<string>() { _parameterlessMethodAnnotation };
            }

            var annotations = new List<string>();
            annotations.Add(_parameterMethodAnnotation);

            var usesBuiltInTypes = parametersList.Any(x1 => x1.Any(x2 => IsBuiltInType(x2.Type)));

            if (usesBuiltInTypes)
            {
                // TODO
                throw new NotImplementedException();
            }

            foreach (var parametersForMethod in parametersList)
            {
                annotations.Add(string.Format(_parameterAnnotationTemplate, string.Join(", ", parametersForMethod.Select(p => p.Value))));
            }

            return annotations;
        }

        private static bool IsBuiltInType(string type) => BuiltInTypes.Any(t => t.Name == type);

        private static List<Type> BuiltInTypes = new()
        {
            // value types
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(uint),
            typeof(nint),
            typeof(nuint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            // reference types
            typeof(object),
            typeof(string),
            // missing dynamic
        };

        // TODO make private, move to base class, change ctors and properties
        public static string TypeToString(Type type) => BuiltInTypesStringValue.TryGetValue(type, out var value) ? value : type.ToString();

        private static Dictionary<Type, string> BuiltInTypesStringValue = new()
        {
            { typeof(int), "int" },
            // TODO complete
        };
    }
}
