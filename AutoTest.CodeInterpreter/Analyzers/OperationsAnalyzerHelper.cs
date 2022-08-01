using AutoTest.CodeInterpreter.Analyzers;
using AutoTest.CodeInterpreter.Enums;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.Core.Helpers;
using AutoTest.Core.Models;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public static class OperationsAnalyzerHelper
    {
        public static void AdjustParameterConstraints(
            LiteralOrParameterDefinition returnParameter,
            Dictionary<string, IConstraint> constraints,
            StatementWrapper statementWrapper)
        {
            switch (statementWrapper.SyntaxNode)
            {
                case IfStatementSyntax ifSyntax:
                    AdjustConstraints(constraints, (BinaryExpressionSyntax)ifSyntax.Condition, statementWrapper.IsElseStatement);
                    break;
                case ExpressionStatementSyntax expressionStatementSyntax:
                    AdjustConstraints(constraints, expressionStatementSyntax);
                    break;
                case LocalDeclarationStatementSyntax localDeclarationStatementSyntax:
                    GetParametersFromDeclaration(localDeclarationStatementSyntax)
                        .ToList()
                        .ForEach(parameter =>
                        {
                            constraints.Add(parameter.Name, NumericHelper.GetConstraintFromType(parameter.Type));
                            SetInitialValue(parameter.Type, constraints[parameter.Name], parameter.Value!);
                        });
                    break;
                case ReturnStatementSyntax returnStatementSyntax:
                    AdjustReturnParameter(constraints, returnParameter, returnStatementSyntax);
                    break;
                case MethodDeclarationSyntax:
                    break;
                case ForStatementSyntax:
                    throw new Exception("this should have been handled before - something's wrong");// TODO
                default:
                    throw new NotImplementedException();
            }
        }

        private static void AdjustConstraints(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, bool IsElseStatement)
        {
            var operators = GetOperators(binaryExpression);
            var kind = binaryExpression.Kind();
            var (type, constraint, variable) = GetOperationTypeAndConstraint(constraints, operators);
            ProcessOperation(kind, binaryExpression, constraint!, type, IsElseStatement, operators.Where(op => op != variable).ToList());
        }

        private static void AdjustConstraints(Dictionary<string, IConstraint> constraints, ExpressionStatementSyntax expressionStatementSyntax)
        {
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
            UpdateValue(NumericHelper.GetNumericTypeFromValue(value), constraints[variableName1], MathOperations.Sum, value);
        }

        private static IEnumerable<Parameter> GetParametersFromDeclaration(LocalDeclarationStatementSyntax localDeclarationStatementSyntax)
        {
            foreach (var variable in localDeclarationStatementSyntax.Declaration.Variables)
            {
                var variableValue = ((LiteralExpressionSyntax)variable.Initializer.Value).Token.Value;
                yield return new Parameter
                {
                    Name = variable.Identifier.ValueText,
                    Value = variableValue,
                    Type = NumericHelper.GetNumericTypeFromValue(variableValue)
                };
            }
        }

        private static void AdjustReturnParameter(Dictionary<string, IConstraint> constraints, LiteralOrParameterDefinition returnParameter, ReturnStatementSyntax returnStatementSyntax)
        {
            switch(returnStatementSyntax.Expression)
            {
                case ExpressionSyntax when returnStatementSyntax.Expression is IdentifierNameSyntax:
                    var returnStatementVariableName = ((IdentifierNameSyntax)returnStatementSyntax.Expression).Identifier.ValueText;
                    returnParameter.Name = returnStatementVariableName;
                    if (!constraints[returnStatementVariableName].IsUndeterminedValue())
                    {
                        returnParameter.Value = constraints[returnStatementVariableName].Generate();
                    }
                    break;
                case ExpressionSyntax when returnStatementSyntax.Expression is LiteralExpressionSyntax:
                    returnParameter.Name = string.Empty;
                    returnParameter.Value = ((LiteralExpressionSyntax)returnStatementSyntax.Expression).Token.Value;
                    returnParameter.IsLiteral = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void SetInitialValue(Type type, IConstraint constraint, object value) 
            => GetOperationAnalyzer(type).AddInitialValue(constraint, value);

        private static void UpdateValue(Type type, IConstraint constraint, MathOperations mathOperation, object value)
            => GetOperationAnalyzer(type).UpdateValue(constraint, mathOperation, value);

        private static void ProcessOperation(SyntaxKind kind, BinaryExpressionSyntax binaryExpression, IConstraint constraint, Type type, bool isElseStatement, IEnumerable<string> operators) 
            => GetOperationAnalyzer(type).AdjustConstraint(constraint, kind, binaryExpression, isElseStatement, operators);

        private static IOperationsAnalyzer GetOperationAnalyzer(Type type) 
            => type switch
            {
                Type when IsNumericOperation(type) => GetNumericalAnalyzer(type),
                Type when IsTextOperation(type) => new TextOperationAnalyzer(),
                _ => throw new NotImplementedException(),//TODO
            };

        private static IOperationsAnalyzer GetNumericalAnalyzer(Type type) 
            => (INumericalAnalyzer)Activator.CreateInstance(typeof(NumericOperationAnalyzer<>).MakeGenericType(type));

        private static bool IsNumericOperation(Type type) => PrimitiveTypeConvertionHelper.NumericalTypes.Contains(type);

        private static bool IsTextOperation(Type type) => type == typeof(string) || type == typeof(char);

        private static (Type type, IConstraint? constraint, string variable) GetOperationTypeAndConstraint(Dictionary<string, IConstraint> constraints, IEnumerable<string> operators)
        {
            foreach(var @operator in operators)
            {
                if(constraints.TryGetValue(@operator, out var constraint)) {
                    return (constraint.GetVariableType(), constraint, @operator);
                }
            }

            throw new NotImplementedException();//TODO
        }

        // TODO: this may need to be refactored in the future for less and more operators
        private static IEnumerable<string> GetOperators(BinaryExpressionSyntax binaryExpression)
        {
            var operators = new List<string>
            {
                binaryExpression.Left.GetText().ToString().Trim(),
                binaryExpression.Right.GetText().ToString().Trim(),
            };

            return operators.ToList();
        }
    }
}
