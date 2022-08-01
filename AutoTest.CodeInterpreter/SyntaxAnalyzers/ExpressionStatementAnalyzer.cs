using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ExpressionStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ExpressionStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = (ExpressionStatementSyntax)statement });
                return new List<CodeExecution>() { executionPath };
            };
    }
}
