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
            Func<string, IEnumerable<(string Name, Type Type, object Value)>, IEnumerable<StatementWrapper>, UnitTest> createUnitTest = GenerateUnitTest(method);

            return codeRunnerService.RunMethod(method)
                .Select(m => 
                {
                    var parameterListWithValues = GenerateParameterListWithValues(method.Parameters, m.ParameterConstraints);
                    return createUnitTest(FormatMethodName(m.Method.Name, namingConvention), parameterListWithValues, m.Path);
                })
                .ToList();
        }

        protected abstract Func<string, IEnumerable<(string Name, Type Type, object Value)>, IEnumerable<StatementWrapper>, UnitTest> GenerateUnitTest(MethodWrapper method);

        private static IEnumerable<(string Name, Type Type, object Value)> GenerateParameterListWithValues(Dictionary<string, Type> parameters, Dictionary<string, IConstraint> constraints)
        {
            foreach (var parameter in parameters)
            {
                yield return (parameter.Key, parameter.Value, constraints[parameter.Key].Generate());
            }
        }

        // TODO: implement
        private static string FormatMethodName(string methodName, TestNamingConventions namingConvention)
        {
            return $"{methodName}_WhenSomething_ShouldSomething";
        }
    }
}
