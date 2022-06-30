using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public class NumericOperationAnalyzer : IOperationsAnalyzer
    {
        public static IEnumerable<SyntaxKind> SupportedOperations
            => new List<SyntaxKind>()
            .Concat(LessThanOrGreaterThanOperations)
            .Concat(EqualityOperations);

        public static bool IsSupported(SyntaxKind kind) => SupportedOperations.Contains(kind);

        // TODO: this elseifs can be improved to a dictionary-like alternative
        public static void AdjustConstraints(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, bool IsElseStatement)
        {
            var kind = binaryExpression.Kind();

            var operator1 = binaryExpression.Left.GetText().ToString().Trim();
            var operator2 = binaryExpression.Right.GetText().ToString().Trim();

            var isVariableInOperator1 = constraints.ContainsKey(operator1);
            var variable = isVariableInOperator1 ? operator1 : operator2;

            var (type, value) = ConvertAndGetVariableType(isVariableInOperator1 ? operator2 : operator1);

            if (LessThanOrGreaterThanOperations.Contains(kind))
            {
                ProcessLessThanOrGreaterThanOperations(type, variable, value, constraints, binaryExpression, kind, IsElseStatement);
                return;
            }
            else if (EqualityOperations.Contains(kind))
            {
                ProcessEqualityOperations(type, variable, value, constraints, binaryExpression, kind, IsElseStatement);
                return;
            }

            throw new NotImplementedException();
        }

        // TODO: implement
        private static (Type, object) ConvertAndGetVariableType(string variable)
        {
            // TODO: implement
            return (typeof(int), int.Parse(variable));
        }

        private static void ProcessEqualityOperations<T>(T type, string variableName, T value, Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, SyntaxKind kind, bool isElseStatement)
        {
            var numericalConstraint = IdentifyOperands(type, variableName, constraints, binaryExpression);

            Action<NumericalConstraint<T>, T> addConstraint = ((kind == SyntaxKind.EqualsExpression && !isElseStatement) || (kind == SyntaxKind.NotEqualsExpression && isElseStatement))
                ? (constraint, value) => { constraint.SetMinValue(value); constraint.SetMaxValue(constraint.SumWithType(value, 1)); }
                : (constraint, value) => constraint.Excluding(value);

            addConstraint(numericalConstraint, value);
        }

        private static void ProcessLessThanOrGreaterThanOperations<T>(T type, string variableName, T value, Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, SyntaxKind kind, bool isElseStatement)
        {
            var numericalConstraint = IdentifyOperands(type, variableName, constraints, binaryExpression);

            var isActingOnIfBranch = kind == SyntaxKind.GreaterThanExpression
                || kind == SyntaxKind.LessThanOrEqualExpression;

            Action<NumericalConstraint<T>, T> addConstraint = !isElseStatement
                ? (constraint, value) => constraint.SetMinValue(constraint.SumWithType(value, (isActingOnIfBranch ? 1 : 0)))
                : (constraint, value) => constraint.SetMaxValue(constraint.SumWithType(value, (!isActingOnIfBranch ? -1 : 0)));

            addConstraint(numericalConstraint, value);
        }

        private static NumericalConstraint<T> IdentifyOperands<T>(T _, string variableName, Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression)
        {
            var numericalConstraint = (NumericalConstraint<T>)constraints[variableName];
            return numericalConstraint;
            //return (numericalConstraint, numericalConstraint.ParseStringToType(valueAsString));
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
