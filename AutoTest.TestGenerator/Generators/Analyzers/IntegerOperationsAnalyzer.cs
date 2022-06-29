using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public class IntegerOperationsAnalyzer : IOperationsAnalyzer
    {
        public static IEnumerable<SyntaxKind> SupportedOperations
            => new List<SyntaxKind>()
            .Concat(LessThanOrGreaterThanOperations)
            .Concat(ComparisonOperations);

        public static bool IsSupported(SyntaxKind kind) => SupportedOperations.Contains(kind);

        // TODO: this elseifs can be improved to a dictionary-like alternative
        public static void AdjustConstraints(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, bool IsElseStatement)
        {
            var kind = binaryExpression.Kind();

            if (LessThanOrGreaterThanOperations.Contains(kind))
            {
                ProcessLessThanOrGreaterThanOperations(constraints, binaryExpression, kind, IsElseStatement);
                return;
            }
            else if (ComparisonOperations.Contains(kind))
            {
                ProcessComparisonOperations(constraints, binaryExpression, kind, IsElseStatement);
                return;
            }

            throw new NotImplementedException();
        }

        private static void ProcessComparisonOperations(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, SyntaxKind kind, bool isElseStatement)
        {
            Action<IntConstraint, int> addConstraint = ((kind == SyntaxKind.EqualsExpression && !isElseStatement) || (kind == SyntaxKind.NotEqualsExpression && isElseStatement))
                ? (constraint, value) => { constraint.SetMinValue(value); constraint.SetMaxValue(value + 1); }
                : (constraint, value) => constraint.Excluding(value);

            var operator1 = binaryExpression.Left.GetText().ToString().Trim();
            var operator2 = binaryExpression.Right.GetText().ToString().Trim();

            var isVariableInOperator1 = constraints.ContainsKey(operator1);
            var variable = isVariableInOperator1 ? operator1 : operator2;
            var value = isVariableInOperator1 ? int.Parse(operator2) : int.Parse(operator1);

            addConstraint((IntConstraint)constraints[variable], value);
        }

        private static void ProcessLessThanOrGreaterThanOperations(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, SyntaxKind kind, bool isElseStatement)
        {
            var isActingOnIfBranch = kind == SyntaxKind.GreaterThanExpression
                || kind == SyntaxKind.LessThanOrEqualExpression;

            Action<IntConstraint, int> addConstraint = !isElseStatement
                ? (constraint, value) => constraint.SetMinValue(value + (isActingOnIfBranch ? 1 : 0))
                : (constraint, value) => constraint.SetMaxValue(value + (!isActingOnIfBranch ? -1 : 0));

            var operator1 = binaryExpression.Left.GetText().ToString().Trim();
            var operator2 = binaryExpression.Right.GetText().ToString().Trim();

            var isVariableInOperator1 = constraints.ContainsKey(operator1);
            var variable = isVariableInOperator1 ? operator1 : operator2;
            var value = isVariableInOperator1 ? int.Parse(operator2) : int.Parse(operator1);

            addConstraint((IntConstraint)constraints[variable], value);
        }

        private static IEnumerable<SyntaxKind> LessThanOrGreaterThanOperations
            => new List<SyntaxKind>()
            {
                SyntaxKind.GreaterThanExpression,
                SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.LessThanExpression,
                SyntaxKind.LessThanOrEqualExpression
            };

        private static IEnumerable<SyntaxKind> ComparisonOperations
            => new List<SyntaxKind>()
            {
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression
            };
    }
}
