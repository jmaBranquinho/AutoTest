using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Services;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.Core.Models;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestMethodGenerator : ITestMethodGenerator
    {
        private static readonly CodeRunnerService codeRunnerService = new();

        public IEnumerable<UnitTestBase> GenerateUnitTests(MethodWrapper method, TestNamingConventions namingConvention = TestNamingConventions.MethodName_WhenCondition_ShouldResult)
        {
            Func<string, IEnumerable<Parameter>, CodeRunExecution, UnitTestBase> createUnitTest = GenerateUnitTest(method);

            return codeRunnerService.RunMethod(method)
                .Select(codeRun => 
                {
                    var parameterListWithValues = GenerateParameterListWithValues(method.Parameters, codeRun.ParameterConstraints);
                    return createUnitTest(FormatMethodName(codeRun.Method.Name, namingConvention), parameterListWithValues, codeRun);
                })
                .ToList();
        }

        protected abstract Func<string, IEnumerable<Parameter>, CodeRunExecution, UnitTestBase> GenerateUnitTest(MethodWrapper method);

        private static IEnumerable<Parameter> GenerateParameterListWithValues(Dictionary<string, Type> parameters, Dictionary<string, IConstraint> constraints)
        {
            foreach (var parameter in parameters)
            {
                yield return new Parameter { Name = parameter.Key, Type = parameter.Value, Value = constraints[parameter.Key].Generate() };
            }
        }

        private static string FormatMethodName(string methodName, TestNamingConventions namingConvention)
        {
            return namingConvention switch
            {
                TestNamingConventions.MethodName_WhenCondition_ShouldResult => $"{methodName}_WhenSomething_ShouldSomething",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
