using AutoTest.CodeInterpreter.Analyzers;
using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public class NumericOperationAnalyzer<T> : INumericalAnalyzer where T : IConvertible
    {
        public static IEnumerable<SyntaxKind> SupportedOperations => AnalyzerHelper.EqualityOperations;

        public static bool IsSupported(SyntaxKind kind) => SupportedOperations.Contains(kind);

        public void AdjustConstraint(IConstraint constraint, SyntaxKind kind, BinaryExpressionSyntax binaryExpression, bool isElseStatement, IEnumerable<string> operators) 
        {
            var isReversedEqualityOperation = (kind == SyntaxKind.NotEqualsExpression && isElseStatement);

            var requiresModification = kind == SyntaxKind.GreaterThanExpression
                || kind == SyntaxKind.LessThanOrEqualExpression;

            var isEqualOrNonEqualOperation = kind == SyntaxKind.EqualsExpression
                || kind == SyntaxKind.NotEqualsExpression;

            Action<NumericalConstraint<T>, T> addConstraint = (!isElseStatement && kind != SyntaxKind.NotEqualsExpression) || isReversedEqualityOperation
                ? (constraint, value) =>
                {
                    constraint.SetMinValue(constraint.SumToUndeterminedValue(value, requiresModification ? SumModifications.IncrementUnit : SumModifications.NoModification));
                    if(isEqualOrNonEqualOperation)
                    {
                        constraint.SetMaxValue(constraint.SumToUndeterminedValue(value, SumModifications.IncrementUnit));
                    }
                }
                : (constraint, value) =>
                {
                    if(!isEqualOrNonEqualOperation)
                    {
                        constraint.SetMaxValue(constraint.SumToUndeterminedValue(value, !requiresModification ? SumModifications.DecrementUnit : SumModifications.NoModification));
                    } 
                    else
                    {
                        constraint.Excluding(value);
                    }
                };

            AnalyzerHelper.ConvertToType<T>(operators.FirstOrDefault(), out var convertedOperator);
            addConstraint((NumericalConstraint<T>)constraint, (T)convertedOperator);
        }

        public void AddInitialValue(IConstraint constraint, object value) => ((NumericalConstraint<T>)constraint).SetInitialValue((T)value);

        public void ModifyKnownValue(IConstraint constraint, object value) => ((NumericalConstraint<T>)constraint).SumToValue((T)value);
    }
}
