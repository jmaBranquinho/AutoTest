using AutoTest.CodeInterpreter.Enums;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface IOperationsAnalyzer
    {
        void AdjustConstraint(IConstraint constraint, SyntaxKind kind, BinaryExpressionSyntax binaryExpression, bool isElseStatement, IEnumerable<string> operators);

        void AddInitialValue(IConstraint constraint, object value);

        void UpdateValue(IConstraint constraint, MathOperations mathOperation, object value);
    }
}
