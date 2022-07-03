using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public class TextOperationAnalyzer : IOperationsAnalyzer
    {
        public void AdjustConstraint(IConstraint constraint, SyntaxKind kind, BinaryExpressionSyntax binaryExpression, bool isElseStatement, IEnumerable<string> operators)
        {
            if (kind == SyntaxKind.NotEqualsExpression)
            {
                isElseStatement = !isElseStatement;
            }

            Action<StringConstraint, string> addConstraint = !isElseStatement
                ? (constraint, value) => constraint.Exactly(value)
                : (constraint, value) => constraint.Excluding(value);

            addConstraint((StringConstraint)constraint, operators.FirstOrDefault());
        }
    }
}
