using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Enums;
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
                NumericOperationAnalyzer<T>.ProcessLessThanOrGreaterThanOperations((NumericalConstraint<T>) constraint, kind, isElseStatement, operators.FirstOrDefault());
            }
            else if (EqualityOperations.Contains(kind))
            {
                NumericOperationAnalyzer<T>.ProcessEqualityOperations((NumericalConstraint<T>)constraint, kind, isElseStatement, operators.FirstOrDefault());
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void ProcessLessThanOrGreaterThanOperations(NumericalConstraint<T> constraint, SyntaxKind kind, bool isElseStatement, string? @operator)
        {
            var isActingOnIfBranch = kind == SyntaxKind.GreaterThanExpression
                || kind == SyntaxKind.LessThanOrEqualExpression;

            Action<NumericalConstraint<T>, T> addConstraint = !isElseStatement
                ? (constraint, value) => constraint.SetMinValue(constraint.SumWithType(value, isActingOnIfBranch ? SumModifications.IncrementUnit : SumModifications.NoModification))
                : (constraint, value) => constraint.SetMaxValue(constraint.SumWithType(value, !isActingOnIfBranch ? SumModifications.DecrementUnit : SumModifications.NoModification));

            addConstraint(constraint, NumericOperationAnalyzer<T>.ConvertToType(@operator));
        }

        private static void ProcessEqualityOperations(NumericalConstraint<T> constraint, SyntaxKind kind, bool isElseStatement, string? @operator)
        {
            Action<NumericalConstraint<T>, T> addConstraint = ((kind == SyntaxKind.EqualsExpression && !isElseStatement) || (kind == SyntaxKind.NotEqualsExpression && isElseStatement))
                ? (constraint, value) => { constraint.SetMinValue(value); constraint.SetMaxValue(constraint.SumWithType(value, SumModifications.IncrementUnit)); }
                : (constraint, value) => constraint.Excluding(value);

            addConstraint(constraint, NumericOperationAnalyzer<T>.ConvertToType(@operator));
        }

        private static T ConvertToType(string value)
        {
            var cleanedValue = value
                .Replace("d", string.Empty)
                .Replace("m", string.Empty);

            if (typeof(T) == typeof(double))
            {
                return (T)(object)double.Parse(cleanedValue);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (T)(object)decimal.Parse(cleanedValue);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

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
