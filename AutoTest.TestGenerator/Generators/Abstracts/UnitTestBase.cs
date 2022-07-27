using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Helpers;
using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.Core.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestBase : Method
    {
        protected abstract string ParameterlessMethodAnnotation { get; }
        protected abstract string ParameterMethodAnnotation { get; }
        protected abstract string ParameterAnnotationTemplate { get; }

        private readonly IEnumerable<Parameter> _unitTestParameters;

        public UnitTestBase(string testName, IEnumerable<Parameter> parameters, CodeRunExecution codeRun)
            : base(testName, Enumerable.Empty<string>(), new List<MethodModifiers> { MethodModifiers.Public }, "void", Enumerable.Empty<ParameterDefinition>(), string.Empty)
        {
            PerformValidations(codeRun.Path);

            var isParameterless = !parameters?.Any() ?? true;
            _unitTestParameters = parameters ?? new List<Parameter>();

            _annotations = isParameterless
                ? new List<string>() { ParameterlessMethodAnnotation }
                : FormatXUnitParameterTestAnnotations(_unitTestParameters);

            _parameters = isParameterless
                ? Enumerable.Empty<string>()
                : FormatXUnitTestMethodParameter(_unitTestParameters);

            _body = FormatXUnitTestBody(codeRun.Path);
        }

        private string FormatXUnitTestBody(IEnumerable<StatementWrapper> methodStatements) 
            => string.Join(Environment.NewLine, GenerateArrangeSection(), GenerateActSection(methodStatements), GenerateAssertSection(methodStatements));

        private string GenerateActSection(IEnumerable<StatementWrapper> methodStatements)
            => WriteSection((stringBuilder) =>
            {
                var methodDeclaration = (MethodDeclarationSyntax)methodStatements.First().SyntaxNode;
                stringBuilder
                    .Append($"var actual = _sut.{methodDeclaration.Identifier.Text}"
                        .AddNewContext(
                            !IsParameterless()
                            ? string.Join(", ", _unitTestParameters.First().Name)
                            : string.Empty, Symbols.Parentheses)
                        .EndStatement(hasLineBreak: true));
            }, sectionTitle: "Act");

        // TODO: implement
        // TODO: assert property changes
        private string GenerateAssertSection(IEnumerable<StatementWrapper> methodStatements)
            => WriteSection((stringBuilder) =>
            {
                if (methodStatements is not null && methodStatements.Any())
                {
                    stringBuilder.Append("Assert.Equal".AddNewContext($"expected, actual", Symbols.Parentheses).EndStatement());
                }
            }, sectionTitle: "Assert");

        // TODO: implement
        private IEnumerable<string> FormatXUnitParameterTestAnnotations(IEnumerable<Parameter> parametersList)
        {
            if (parametersList is null || !parametersList.Any())
            {
                return new List<string>() { ParameterlessMethodAnnotation };
            }

            var annotations = new List<string>
            {
                ParameterMethodAnnotation
            };

            var isNotUsingBuiltInTypes = parametersList.Any(x1 => !IsBuiltInType(x1.Type));

            if (isNotUsingBuiltInTypes)
            {
                // TODO
                throw new NotImplementedException();
            }

            parametersList.ToList()
                .ForEach(parametersForMethod => annotations.Add(string.Format(ParameterAnnotationTemplate, string.Join(", ", parametersForMethod.Value))));

            return annotations;
        }

        // TODO: implement
        private static string GenerateArrangeSection()
            => WriteSection((stringBuilder) => { }, sectionTitle: "Arrange");

        private static IEnumerable<string> FormatXUnitTestMethodParameter(IEnumerable<Parameter> parametersList) 
            => FormatParameters(parametersList);

        private static bool IsBuiltInType(Type type) => PrimitiveTypeConvertionHelper.PrimitiveTypes.Any(t => t == type);

        private static string WriteSection(Action<StringBuilder> writeSection, string sectionTitle)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"// {sectionTitle}");

            writeSection(stringBuilder);

            return stringBuilder.ToString();
        }

        private static void PerformValidations(IEnumerable<StatementWrapper> methodStatements)
        {
            if (methodStatements is null || !methodStatements.Any())
            {
                throw new Exception("no method statements"); // TODO
            }
        }
    }
}
