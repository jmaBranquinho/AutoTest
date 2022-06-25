using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestMethodGenerator : ITestMethodGenerator
    {
        public IEnumerable<Method> GenerateUnitTests(MethodWrapper method, TestNamingConventions namingConvention)
        {
            Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, Method> createUnitTest = GenerateUnitTest(method);

            return method.ExecutionPaths.Select(path => 
                createUnitTest(FormatMethodName(method.Name, namingConvention), GenerateParameters(method.Parameters, path), path))
                    .ToList();
        }

        protected abstract Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, Method> GenerateUnitTest(MethodWrapper method);

        // TODO: implement the rest
        private IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> GenerateParameters(Dictionary<string, Type> parameters, IEnumerable<StatementWrapper> path)
        {
            var methodStatements = path.Skip(1).ToList();

            var constraints = new Dictionary<string, IConstraint>();
            PopulateParameterConstraints(constraints, parameters);

            foreach (var statement in methodStatements)
            {
                AdjustParameterConstraints(statement, parameters);
            }

            var parameterListWithValues = GenerateParameterListWithValues(parameters, constraints);

            return new List<List<(string Name, Type Type, object Value)>> { parameterListWithValues.ToList() };
        }

        private IEnumerable<(string Name, Type Type, object Value)> GenerateParameterListWithValues(Dictionary<string, Type> parameters, Dictionary<string, IConstraint> constraints)
        {
            foreach (var parameter in parameters)
            {
                yield return (parameter.Key, parameter.Value, constraints[parameter.Key].Generate());
            }
        }

        // TODO: implement for other types
        private void PopulateParameterConstraints(Dictionary<string, IConstraint> constraints, Dictionary<string, Type> parameters)
        {
            foreach (var parameter in parameters)
            {
                // TODO see above
                constraints.Add(parameter.Key, new IntConstraint());
            }
        }

        // TODO: implement
        private void AdjustParameterConstraints(StatementWrapper statementWrapper, Dictionary<string, Type> parameters)
        {
            // TODO: implement
        }

        // TODO: implement
        private static string FormatMethodName(string methodName, TestNamingConventions namingConvention)
        {
            return methodName;
        }
    }
}
