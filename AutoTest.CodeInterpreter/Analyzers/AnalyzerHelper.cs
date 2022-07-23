using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;

namespace AutoTest.CodeInterpreter.Analyzers
{
    public static class AnalyzerHelper
    {
        private static readonly Dictionary<Type, Func<IConstraint>> TypeToConstraintDictionary =
            new()
            {
                { typeof(int), () => new IntConstraint() },
                { typeof(double), () => new DoubleConstraint() },
                { typeof(decimal), () => new DecimalConstraint() },
                { typeof(string), () => new StringConstraint() },
            };

        public static IConstraint GetConstraintFromType(Type type) => TypeToConstraintDictionary[type]();

        public static IEnumerable<SyntaxKind> EqualityOperations
            => new List<SyntaxKind>()
            {
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression,
                SyntaxKind.GreaterThanExpression,
                SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.LessThanExpression,
                SyntaxKind.LessThanOrEqualExpression
            };
    }
}