using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestMethodGenerator : ITestMethodGenerator
    {
        public IEnumerable<UnitTest> GenerateUnitTests(MethodWrapper method, TestNamingConventions namingConvention)
        {
            Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, UnitTest> createUnitTest = GenerateUnitTest(method);

            return method.ExecutionPaths.Select(path => 
                createUnitTest(FormatMethodName(method.Name, namingConvention), GenerateParameters(method.Parameters, path), path))
                    .ToList();
        }

        protected abstract Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, UnitTest> GenerateUnitTest(MethodWrapper method);

        // TODO: implement the rest
        private IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> GenerateParameters(Dictionary<string, Type> parameters, IEnumerable<StatementWrapper> path)
        {
            var methodStatements = path.Skip(1).ToList();

            var constraints = new Dictionary<string, IConstraint>();
            PopulateParameterConstraints(constraints, parameters);

            foreach (var statement in methodStatements)
            {
                AdjustParameterConstraints(statement, constraints);
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
        // TODO: extract
        private static void AdjustParameterConstraints(StatementWrapper statementWrapper, Dictionary<string, IConstraint> constraints)
        {
            switch (statementWrapper.SyntaxNode)
            {
                case IfStatementSyntax ifSyntax:

                    var binaryExpression = (BinaryExpressionSyntax) ifSyntax.Condition;
                    var kind = binaryExpression.Kind();

                    var isActingOnIfBranch = kind == SyntaxKind.GreaterThanExpression
                        || kind == SyntaxKind.LessThanOrEqualExpression;

                    Action<IntConstraint, int> addConstraint = !statementWrapper.IsElseStatement
                        ? (constraint, value) => constraint.SetMinValue(value + (isActingOnIfBranch ? 1 : 0))
                        : (constraint, value) => constraint.SetMaxValue(value + (!isActingOnIfBranch ? -1 : 0));
                    
                    var operator1 = binaryExpression.Left.GetText().ToString().Trim();
                    var operator2 = binaryExpression.Right.GetText().ToString().Trim();

                    var isVariableInOperator1 = constraints.ContainsKey(operator1);
                    var variable = isVariableInOperator1 ? operator1 : operator2;
                    var value = isVariableInOperator1 ? int.Parse(operator2) : int.Parse(operator1);

                    addConstraint((IntConstraint) constraints[variable], value);
                    break;
                default:
                    break;
                    //throw new NotImplementedException();
            }
        }

        // TODO: implement
        private static string FormatMethodName(string methodName, TestNamingConventions namingConvention)
        {
            return $"{methodName}_WhenSomething_ShouldSomething";
        }
    }
}
