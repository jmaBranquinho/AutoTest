using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTest : Method
    {
        protected abstract string _parameterlessMethodAnnotation { get; }
        protected abstract string _parameterMethodAnnotation { get; }
        protected abstract string _parameterAnnotationTemplate { get; }

        private readonly IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> _unitTestParameters;

        public UnitTest(string name, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parameters, IEnumerable<StatementWrapper> methodStatements) 
            : base(name, Enumerable.Empty<string>(), new List<MethodModifiers> { MethodModifiers.Public }, "void", Enumerable.Empty<(string Name, Type Type) >(), string.Empty)
        {
            var isParameterless = !parameters?.Any() ?? true;
            _unitTestParameters = parameters;

            _annotations = isParameterless
                ? new List<string>() { _parameterlessMethodAnnotation }
                : FormatXUnitParameterTestAnnotations(parameters);

            _parameters = isParameterless
                ? Enumerable.Empty<string>()
                : FormatXUnitTestMethodParameter(parameters);

            _body = FormatXUnitTestBody(methodStatements);
        }

        private string FormatXUnitTestBody(IEnumerable<StatementWrapper> methodStatements) 
            => string.Join(Environment.NewLine, GenerateArrangeSection(), GenerateActSection(methodStatements), GenerateAssertSection(methodStatements));

        // TODO: check for issues
        private string GenerateActSection(IEnumerable<StatementWrapper> methodStatements)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// Act");

            if (methodStatements is null || !methodStatements.Any())
            {
                // TODO
                throw new Exception("no method statements");
            }

            var methodDeclaration = (MethodDeclarationSyntax)methodStatements.First().SyntaxNode;
            stringBuilder.Append($"var actual = _sut.{methodDeclaration.Identifier.Text}(");

            if (!IsParameterless())
            {
                stringBuilder.Append(string.Join(", ", _unitTestParameters.First().Select(p => p.Name).ToList()));
            }
            stringBuilder.AppendLine(");");

            return stringBuilder.ToString();
        }

        // TODO: implement
        // TODO: assert property changes
        private string GenerateAssertSection(IEnumerable<StatementWrapper> methodStatements)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// Assert");
            if (methodStatements is not null && methodStatements.Any())
            {
                stringBuilder.Append("Assert.Equal(");
                stringBuilder.Append("Assert.Equal(expected, actual");
                stringBuilder.Append(");");
            }

            return stringBuilder.ToString();
        }

        // TODO: implement
        private IEnumerable<string> FormatXUnitParameterTestAnnotations(IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parametersList)
        {
            if (parametersList is null || !parametersList.Any())
            {
                return new List<string>() { _parameterlessMethodAnnotation };
            }

            var annotations = new List<string>();
            annotations.Add(_parameterMethodAnnotation);

            var isNotUsingBuiltInTypes = parametersList.Any(x1 => x1.Any(x2 => !IsBuiltInType(x2.Type)));

            if (isNotUsingBuiltInTypes)
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

        // TODO: implement
        private static string GenerateArrangeSection()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// Arrange");
            // TODO: implement

            return stringBuilder.ToString();
        }

        private static IEnumerable<string> FormatXUnitTestMethodParameter(IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parametersList) 
            => FormatParameters(parametersList.First().Select(p => (p.Name, p.Type)).ToList());

        private static bool IsBuiltInType(Type type) => BuiltInTypes.Any(t => t == type);

        // TODO: implement
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
    }
}
