using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Analyzers;
using AutoTest.CodeInterpreter.Enums;
using AutoTest.CodeInterpreter.Models;
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
        public IEnumerable<CodeRunExecution> RunMethod(MethodWrapper method) 
            => method.ExecutionPaths.Select(path 
                => IterateMethodStatements(method, path));

        private static CodeRunExecution IterateMethodStatements(MethodWrapper method, IEnumerable<StatementWrapper> path)
        {
            var methodStatements = path.Skip(1).ToList();

            var parameterConstraints = new Dictionary<string, IConstraint>();
            PopulateParameterConstraints(parameterConstraints, method.Parameters);
            var returnParameter = GetMethodReturnType(path.First());

            methodStatements.ForEach(statement => AdjustParameterConstraints(statement, returnParameter, parameterConstraints));

            return new CodeRunExecution
            {
                Method = method,
                Path = path,
                ParameterConstraints = parameterConstraints,
                ReturnParameter = returnParameter,
            };
        }

        private static Parameter GetMethodReturnType(StatementWrapper statementWrapper)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax)statementWrapper.SyntaxNode;
            var returnTypeAsString = ((PredefinedTypeSyntax)methodDeclarationSyntax.ReturnType).Keyword.ValueText;
            return new Parameter { Type = PrimitiveTypeConvertionHelper.GetTypeFromString(returnTypeAsString) };
        }

        private static void AdjustParameterConstraints(
            StatementWrapper statementWrapper,
            Parameter returnParameter,
            Dictionary<string, IConstraint> constraints)
        {
            switch (statementWrapper.SyntaxNode)
            {
                case IfStatementSyntax ifSyntax:
                    OperationsAnalyzerHelper.AdjustConstraints(constraints, (BinaryExpressionSyntax)ifSyntax.Condition, statementWrapper.IsElseStatement);
                    break;
                case ExpressionStatementSyntax expressionStatementSyntax:
                    var expression = expressionStatementSyntax.Expression;
                    var expressionKind = expression.Kind();

                    var isNumericExpressionKind = expressionKind == SyntaxKind.PostIncrementExpression
                        || expressionKind == SyntaxKind.PostDecrementExpression;

                    if (!isNumericExpressionKind)
                    {
                        throw new NotImplementedException();
                    }

                    object? value = expressionKind switch
                    {
                        SyntaxKind.PostIncrementExpression => 1,
                        SyntaxKind.PostDecrementExpression => -1,
                        _ => throw new NotImplementedException(),
                    };
                    var variableName1 = ((IdentifierNameSyntax)((PostfixUnaryExpressionSyntax)expression).Operand).Identifier.ValueText;
                    OperationsAnalyzerHelper.UpdateValue(NumericHelper.GetNumericTypeFromValue(value), constraints[variableName1], MathOperations.Sum, value);
                    break;
                case LocalDeclarationStatementSyntax localDeclarationStatementSyntax:

                    var variableDeclaration = localDeclarationStatementSyntax
                            .Declaration
                            .Variables.First();//TODO: turn into a loop
                    var variableName = variableDeclaration.Identifier.ValueText;
                    var variableValue = ((LiteralExpressionSyntax)variableDeclaration.Initializer.Value).Token.Value;
                    var variableType = NumericHelper.GetNumericTypeFromValue(variableValue);

                    constraints.Add(variableName, NumericHelper.GetConstraintFromType(variableType));
                    OperationsAnalyzerHelper.SetInitialValue(variableType, constraints[variableName], variableValue);
                    break;
                case ReturnStatementSyntax returnStatementSyntax:
                    if(returnStatementSyntax.Expression is IdentifierNameSyntax identifierNameSyntax)
                    {
                        var returnStatementVariableName = identifierNameSyntax.Identifier.ValueText;
                        returnParameter.Name = returnStatementVariableName;
                        if(!constraints[returnStatementVariableName].IsUndeterminedValue())
                        {
                            returnParameter.Value = constraints[returnStatementVariableName].Generate();
                        }
                    } 
                    else
                    {
                        //TODO: literal expression
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

        private static void PopulateParameterConstraints(Dictionary<string, IConstraint> constraints, Dictionary<string, Type> parameters)
            => parameters.ToList()
                .ForEach(parameter => constraints.Add(parameter.Key, NumericHelper.GetConstraintFromType(parameter.Value)));
    }
}
