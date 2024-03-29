﻿using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.Core.Enums;
using AutoTest.Core.Helpers;
using AutoTest.Core.Models;
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

        public UnitTestBase(string testName, IEnumerable<Parameter> parameters, ExecutionPathInfo executionPathInfo)
            : base(testName, Enumerable.Empty<string>(), new List<MethodModifiers> { MethodModifiers.Public }, returnType: null, Enumerable.Empty<ParameterDefinition>(), string.Empty)
        {
            PerformValidations(executionPathInfo);

            var isParameterless = !parameters?.Any() ?? true;
            _unitTestParameters = parameters ?? new List<Parameter>();

            _annotations = isParameterless
                ? new List<string>() { ParameterlessMethodAnnotation }
                : FormatXUnitParameterTestAnnotations(_unitTestParameters);

            _parameters = isParameterless
                ? Enumerable.Empty<string>()
                : FormatXUnitTestMethodParameter(_unitTestParameters);

            _body = FormatXUnitTestBody(executionPathInfo);
        }

        private string FormatXUnitTestBody(ExecutionPathInfo executionPathInfo) 
            => string.Join(Environment.NewLine, GenerateArrangeSection(executionPathInfo), GenerateActSection(executionPathInfo), GenerateAssertSection(executionPathInfo));

        private static string GenerateArrangeSection(ExecutionPathInfo run)
            => WriteSection((stringBuilder) => 
            {
                if (run.ReturnParameter is not null && run.ReturnParameter.Value is not null)
                {
                    var returnConstraintKeyValuePair = run.ParameterConstraints.FirstOrDefault(constraint => constraint.Key == run.ReturnParameter.Name);
                    var isReturnDeterminedAndRequiresVar = !run.ReturnParameter.IsLiteral && !returnConstraintKeyValuePair.Value.IsUndeterminedValue();

                    if(isReturnDeterminedAndRequiresVar)
                    {
                        stringBuilder.AppendLine($"var expected = {run.ReturnParameter.Value};");
                    }
                }
            }, sectionTitle: "Arrange");

        private string GenerateActSection(ExecutionPath methodStatements)
            => WriteSection((stringBuilder) =>
            {
                var methodDeclaration = (MethodDeclarationSyntax)methodStatements.First().SyntaxNode;
                stringBuilder
                    .Append($"var actual = _sut.{methodDeclaration.Identifier.Text}"
                        .AddNewContext(
                            !IsParameterless()
                            ? _unitTestParameters.First().Name
                            : string.Empty, Symbols.Parentheses)
                        .EndStatement(hasLineBreak: true));
            }, sectionTitle: "Act");

        // TODO: assert property changes
        private string GenerateAssertSection(ExecutionPath methodStatements)
            => WriteSection((stringBuilder) =>
            {
                if (methodStatements is not null && methodStatements.Any())
                {
                    stringBuilder.Append("Assert.Equal".AddNewContext($"expected, actual", Symbols.Parentheses).EndStatement());
                }
            }, sectionTitle: "Assert");

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
                .ForEach(parametersForMethod => annotations.Add(string.Format(ParameterAnnotationTemplate, parametersForMethod.Value)));

            return annotations;
        }

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

        private static void PerformValidations(ExecutionPath methodStatements)
        {
            if (methodStatements is null || !methodStatements.Any())
            {
                throw new Exception("no method statements"); // TODO
            }
        }
    }
}
