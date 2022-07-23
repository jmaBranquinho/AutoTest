using AutoTest.CodeInterpreter.Services;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestMethodGenerator : ITestMethodGenerator
    {
        private static readonly CodeRunnerService codeRunnerService = new();

        public IEnumerable<UnitTest> GenerateUnitTests(MethodWrapper method, TestNamingConventions namingConvention = TestNamingConventions.MethodName_WhenCondition_ShouldResult)
        {
            Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, UnitTest> createUnitTest = GenerateUnitTest(method);

            return CodeRunnerService.RunMethod(method)
                .Select(x => createUnitTest(FormatMethodName(x.Method.Name, namingConvention), x.Parameters, x.Path))
                .ToList();
        }

        protected abstract Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, UnitTest> GenerateUnitTest(MethodWrapper method);

        // TODO: implement
        private static string FormatMethodName(string methodName, TestNamingConventions namingConvention)
        {
            return $"{methodName}_WhenSomething_ShouldSomething";
        }
    }
}
