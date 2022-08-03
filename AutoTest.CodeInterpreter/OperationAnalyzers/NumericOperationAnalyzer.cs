using AutoTest.CodeInterpreter.Enums;
using AutoTest.CodeInterpreter.OperationAnalyzers.Helpers;
using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.OperationAnalyzers
{
    public class NumericOperationAnalyzer<T> : INumericalAnalyzer
        where T : struct,
            IComparable,
            IComparable<T>,
            IConvertible,
            IEquatable<T>,
            IFormattable
    {
        public static IEnumerable<SyntaxKind> SupportedOperations => NumericOperationHelper.EqualityOperations;

        public static bool IsSupported(SyntaxKind kind) => SupportedOperations.Contains(kind);

        public void AdjustConstraint(IConstraint constraint, SyntaxKind kind, BinaryExpressionSyntax binaryExpression, bool isElseStatement, IEnumerable<string> operators)
        {
            var isReversedEqualityOperation = kind == SyntaxKind.NotEqualsExpression && isElseStatement;

            var requiresModification = kind == SyntaxKind.GreaterThanExpression
                || kind == SyntaxKind.LessThanOrEqualExpression;

            var isEqualOrNonEqualOperation = kind == SyntaxKind.EqualsExpression
                || kind == SyntaxKind.NotEqualsExpression;

            Action<NumericalConstraint<T>, T> addConstraint = !isElseStatement && kind != SyntaxKind.NotEqualsExpression || isReversedEqualityOperation
                ? (constraint, value) =>
                {
                    constraint.SetMinValue(constraint.PerformMathOperation(MathOperations.Sum, value, requiresModification ? AbstractedNumericValues.One : AbstractedNumericValues.Zero));
                    if (isEqualOrNonEqualOperation)
                    {
                        constraint.SetMaxValue(constraint.PerformMathOperation(MathOperations.Sum, value, AbstractedNumericValues.One));
                    }
                }
            : (constraint, value) =>
            {
                if (!isEqualOrNonEqualOperation)
                {
                    constraint.SetMaxValue(constraint.PerformMathOperation(MathOperations.Sum, value, !requiresModification ? AbstractedNumericValues.MinusOne : AbstractedNumericValues.Zero));
                }
                else
                {
                    constraint.Excluding(value);
                }
            };

            NumericOperationHelper.ConvertToType<T>(operators.FirstOrDefault(), out var convertedOperator);
            addConstraint((NumericalConstraint<T>)constraint, (T)convertedOperator);
        }

        public void AddInitialValue(IConstraint constraint, object value) => ((NumericalConstraint<T>)constraint).SetInitialValue((T)value);

        public void UpdateValue(IConstraint constraint, MathOperations mathOperation, object value) => ((NumericalConstraint<T>)constraint).PerformMathOperationOnValue(mathOperation, (T)value);
    }
}
