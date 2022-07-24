using AutoTest.CodeInterpreter.Analyzers;
using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.ValueTrackers;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.Core.Helpers;
using AutoTest.TestGenerator.Generators.Analyzers;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Services
{
    /// <summary>
    /// Goes throught the execution paths of a method, trying to identify variables, constraints for parameters and variables and possible return value
    /// </summary>
    public class CodeRunnerService
    {
        public static IEnumerable<CodeRunExecution> RunMethod(MethodWrapper method) 
            => method.ExecutionPaths.Select(path 
                => IterateMethodStatements(method, path));

        private static CodeRunExecution IterateMethodStatements(MethodWrapper method, IEnumerable<StatementWrapper> path)
        {
            var methodStatements = path.Skip(1).ToList();

            var parameterConstraints = new Dictionary<string, IConstraint>();
            var variableConstraints = new Dictionary<string, IValueTracker>();
            PopulateParameterConstraints(parameterConstraints, method.Parameters);
            var returnInfo = GetMethodReturnType(path.First());

            methodStatements.ForEach(statement => AdjustParameterConstraints(statement, returnInfo, parameterConstraints, variableConstraints));

            var parameterListWithValues = GenerateParameterListWithValues(method.Parameters, parameterConstraints);

            return new CodeRunExecution
            {
                Method = method,
                Path = path,
                Parameters = new List<List<(string Name, Type Type, object Value)>> { parameterListWithValues.ToList() },
            };
        }

        private static (string name, Type type, object? value) GetMethodReturnType(StatementWrapper statementWrapper)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax)statementWrapper.SyntaxNode;
            var variableName = methodDeclarationSyntax.Identifier.ValueText;
            var returnTypeAsString = ((PredefinedTypeSyntax)methodDeclarationSyntax.ReturnType).Keyword.ValueText;
            return (variableName, PrimitiveTypeConvertionHelper.GetTypeFromString(returnTypeAsString), null);
        }

        // TODO: implement
        // TODO: extract
        private static void AdjustParameterConstraints(
            StatementWrapper statementWrapper,
            (string name, Type type, object? value) returnInfo,
            Dictionary<string, IConstraint> constraints, 
            Dictionary<string, IValueTracker> variableConstraints)
        {
            switch (statementWrapper.SyntaxNode)
            {
                case IfStatementSyntax ifSyntax:
                    OperationsAnalyzer.AdjustConstraints(constraints, (BinaryExpressionSyntax)ifSyntax.Condition, statementWrapper.IsElseStatement);
                    break;
                case ExpressionStatementSyntax:
                case LocalDeclarationStatementSyntax:
                    if (statementWrapper.SyntaxNode is LocalDeclarationStatementSyntax localDeclarationStatementSyntax)
                    {
                        var variableDeclaration = localDeclarationStatementSyntax
                            .Declaration
                            .Variables.First();
                        var variableName = variableDeclaration.Identifier.ValueText;
                        var variableValue = ((LiteralExpressionSyntax)variableDeclaration.Initializer.Value).Token.Value;
                        variableConstraints.Add(variableName, new UndeterminedNumericValueTracker(new UnderterminedNumericValue { InitialValue = variableValue}));
                    }
                    else if (statementWrapper.SyntaxNode is ExpressionStatementSyntax expressionStatementSyntax)
                    {
                        var expression = expressionStatementSyntax.Expression;
                        var expressionKind = expression.Kind();

                        var isNumericExpressionKind = expressionKind == SyntaxKind.PostIncrementExpression 
                            || expressionKind == SyntaxKind.PostDecrementExpression;
                        if (isNumericExpressionKind)
                        {
                            var operation = (UndeterminedNumericValueTracker tracker) =>
                            {
                                switch (expressionKind)
                                {
                                    case SyntaxKind.PostIncrementExpression:
                                        tracker.Increment();
                                        break;
                                    case SyntaxKind.PostDecrementExpression:
                                        tracker.Decrement();
                                        break;
                                    default: throw new NotImplementedException();
                                }
                            };
                            var variableName = ((IdentifierNameSyntax)((PostfixUnaryExpressionSyntax)expression).Operand).Identifier.ValueText;
                            operation((UndeterminedNumericValueTracker)variableConstraints[variableName]);
                        }
                    }
                    break;
                case ReturnStatementSyntax:
                    if(variableConstraints.ContainsKey(returnInfo.name))
                    {
                        returnInfo.value = variableConstraints[returnInfo.name].TryConvertValue(returnInfo.type);
                    }
                    break;
                case MethodDeclarationSyntax:
                    break;
                case ForStatementSyntax:
                    throw new Exception("this should have been handled before - something's wrong");// TODO
                default:
                    throw new NotImplementedException();
            }
        }

        // This should be extracted - not concerned to this service
        private static IEnumerable<(string Name, Type Type, object Value)> GenerateParameterListWithValues(Dictionary<string, Type> parameters, Dictionary<string, IConstraint> constraints)
        {
            foreach (var parameter in parameters)
            {
                yield return (parameter.Key, parameter.Value, constraints[parameter.Key].Generate());
            }
        }

        // TODO: implement for other types
        private static void PopulateParameterConstraints(Dictionary<string, IConstraint> constraints, Dictionary<string, Type> parameters)
            => parameters.ToList()
                .ForEach(parameter => constraints.Add(parameter.Key, AnalyzerHelper.GetConstraintFromType(parameter.Value)));
    }
}
