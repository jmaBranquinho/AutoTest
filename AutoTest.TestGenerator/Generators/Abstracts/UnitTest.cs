using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Helpers;
using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.Core.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTest : Method
    {
        protected abstract string ParameterlessMethodAnnotation { get; }
        protected abstract string ParameterMethodAnnotation { get; }
        protected abstract string ParameterAnnotationTemplate { get; }

        private readonly IEnumerable<(string Name, Type Type, object Value)> _unitTestParameters;

        public UnitTest(string name, IEnumerable<(string Name, Type Type, object Value)> parameters, IEnumerable<StatementWrapper> methodStatements) 
            : base(name, Enumerable.Empty<string>(), new List<MethodModifiers> { MethodModifiers.Public }, "void", Enumerable.Empty<(string Name, Type Type) >(), string.Empty)
        {
            var isParameterless = !parameters?.Any() ?? true;
            _unitTestParameters = parameters ?? new List<(string Name, Type Type, object Value)>();

            _annotations = isParameterless
                ? new List<string>() { ParameterlessMethodAnnotation }
                : FormatXUnitParameterTestAnnotations(_unitTestParameters);

            _parameters = isParameterless
                ? Enumerable.Empty<string>()
                : FormatXUnitTestMethodParameter(_unitTestParameters);

            _body = FormatXUnitTestBody(methodStatements);
        }

        private string FormatXUnitTestBody(IEnumerable<StatementWrapper> methodStatements) 
            => string.Join(Environment.NewLine, GenerateArrangeSection(), GenerateActSection(methodStatements), GenerateAssertSection(methodStatements));

        private string GenerateActSection(IEnumerable<StatementWrapper> methodStatements)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// Act");

            if (methodStatements is null || !methodStatements.Any())
            {
                throw new Exception("no method statements"); // TODO
            }

            var methodDeclaration = (MethodDeclarationSyntax)methodStatements.First().SyntaxNode;
            stringBuilder
                .Append($"var actual = _sut.{methodDeclaration.Identifier.Text}"
                    .AddNewContext(
                        !IsParameterless() 
                        ? string.Join(", ", _unitTestParameters.First().Name) 
                        : string.Empty, Symbols.Parentheses)
                    .EndStatement(hasLineBreak: true));

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
        private IEnumerable<string> FormatXUnitParameterTestAnnotations(IEnumerable<(string Name, Type Type, object Value)> parametersList)
        {
            if (parametersList is null || !parametersList.Any())
            {
                return new List<string>() { ParameterlessMethodAnnotation };
            }

            var annotations = new List<string>();
            annotations.Add(ParameterMethodAnnotation);

            var isNotUsingBuiltInTypes = parametersList.Any(x1 => !IsBuiltInType(x1.Type));

            if (isNotUsingBuiltInTypes)
            {
                // TODO
                throw new NotImplementedException();
            }

            foreach (var parametersForMethod in parametersList)
            {
                annotations.Add(string.Format(ParameterAnnotationTemplate, string.Join(", ", parametersForMethod.Value)));
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

        private static IEnumerable<string> FormatXUnitTestMethodParameter(IEnumerable<(string Name, Type Type, object Value)> parametersList) 
            => FormatParameters(parametersList.Select(p => (p.Name, p.Type)).ToList());

        private static bool IsBuiltInType(Type type) => PrimitiveTypeConvertionHelper.PrimitiveTypes.Any(t => t == type);
    }
}
