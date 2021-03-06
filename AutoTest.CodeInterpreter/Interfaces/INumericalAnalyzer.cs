using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface INumericalAnalyzer : IOperationsAnalyzer
    {
        void AdjustConstraint(IConstraint constraint, SyntaxKind kind, BinaryExpressionSyntax binaryExpression, bool isElseStatement, IEnumerable<string> operators);
    }
}
