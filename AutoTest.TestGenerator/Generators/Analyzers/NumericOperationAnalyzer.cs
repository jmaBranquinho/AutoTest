using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public class NumericOperationAnalyzer<T> : INumericalAnalyzer where T : IConvertible
    {
        public static IEnumerable<SyntaxKind> SupportedOperations
            => new List<SyntaxKind>()
            .Concat(LessThanOrGreaterThanOperations)
            .Concat(EqualityOperations);

        public static bool IsSupported(SyntaxKind kind) => SupportedOperations.Contains(kind);

        public void AdjustConstraint(IConstraint constraint, SyntaxKind kind, BinaryExpressionSyntax binaryExpression, bool isElseStatement, IEnumerable<string> operators)
        {
            if (LessThanOrGreaterThanOperations.Contains(kind))
            {
                ProcessLessThanOrGreaterThanOperations2((NumericalConstraint<T>) constraint, kind, isElseStatement, operators.FirstOrDefault());
                return;
            }
            else if (EqualityOperations.Contains(kind))
            {
                ProcessEqualityOperations2((NumericalConstraint<T>)constraint, kind, isElseStatement, operators.FirstOrDefault());
                return;
            }

            throw new NotImplementedException();
        }

        private void ProcessLessThanOrGreaterThanOperations2(NumericalConstraint<T> constraint, SyntaxKind kind, bool isElseStatement, string? @operator)
        {
            var isActingOnIfBranch = kind == SyntaxKind.GreaterThanExpression
                || kind == SyntaxKind.LessThanOrEqualExpression;

            Action<NumericalConstraint<T>, T> addConstraint = !isElseStatement
                ? (constraint, value) => constraint.SetMinValue(constraint.SumWithType(value, (isActingOnIfBranch ? 1 : 0)))
                : (constraint, value) => constraint.SetMaxValue(constraint.SumWithType(value, (!isActingOnIfBranch ? -1 : 0)));

            addConstraint(constraint, NumericOperationAnalyzer<T>.ConvertToType(@operator));
        }

        private void ProcessEqualityOperations2(NumericalConstraint<T> constraint, SyntaxKind kind, bool isElseStatement, string? @operator)
        {
            Action<NumericalConstraint<T>, T> addConstraint = ((kind == SyntaxKind.EqualsExpression && !isElseStatement) || (kind == SyntaxKind.NotEqualsExpression && isElseStatement))
                ? (constraint, value) => { constraint.SetMinValue(value); constraint.SetMaxValue(constraint.SumWithType(value, 1)); }
                : (constraint, value) => constraint.Excluding(value);

            addConstraint(constraint, NumericOperationAnalyzer<T>.ConvertToType(@operator));
        }

        private static T ConvertToType(string value) => (T)Convert.ChangeType(value, typeof(T));

        private static IEnumerable<SyntaxKind> LessThanOrGreaterThanOperations
            => new List<SyntaxKind>()
            {
                SyntaxKind.GreaterThanExpression,
                SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.LessThanExpression,
                SyntaxKind.LessThanOrEqualExpression
            };

        private static IEnumerable<SyntaxKind> EqualityOperations
            => new List<SyntaxKind>()
            {
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression
            };
    }
}
