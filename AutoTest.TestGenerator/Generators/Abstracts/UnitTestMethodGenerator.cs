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
        private void AdjustParameterConstraints(StatementWrapper statementWrapper, Dictionary<string, IConstraint> constraints)
        {
            switch (statementWrapper.SyntaxNode)
            {
                case IfStatementSyntax ifSyntax:
                    if (!statementWrapper.IsElseStatement)
                    { 
                        var binaryExpression = (BinaryExpressionSyntax) ifSyntax.Condition;
                        if (binaryExpression.Kind() == SyntaxKind.GreaterThanExpression)
                        {
                            var operator1 = binaryExpression.Left.GetText().ToString().Trim();
                            var operator2 = binaryExpression.Right.GetText().ToString().Trim();

                            string variable;
                            int value;
                            if (constraints.ContainsKey(operator1))
                            {
                                variable = operator1;
                                value = int.Parse(operator2);
                            } 
                            else
                            {
                                variable = operator2;
                                value = int.Parse(operator1);
                            }
                            ((IntConstraint) constraints[variable]).SetMinValue(value);
                        }
                    }
                    else
                    {
                        var binaryExpression = (BinaryExpressionSyntax)ifSyntax.Condition;
                        if (binaryExpression.Kind() == SyntaxKind.GreaterThanExpression)
                        {
                            var operator1 = binaryExpression.Left.GetText().ToString().Trim();
                            var operator2 = binaryExpression.Right.GetText().ToString().Trim();

                            string variable;
                            int value;
                            if (constraints.ContainsKey(operator1))
                            {
                                variable = operator1;
                                value = int.Parse(operator2);
                            }
                            else
                            {
                                variable = operator2;
                                value = int.Parse(operator1);
                            }
                            ((IntConstraint)constraints[variable]).SetMaxValue(value);
                        }
                    }
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
