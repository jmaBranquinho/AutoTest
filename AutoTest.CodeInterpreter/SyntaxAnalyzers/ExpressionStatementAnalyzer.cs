using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ExpressionStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ExpressionStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, List<CodeExecution>>, List<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var expressionStatement = (ExpressionStatementSyntax)statement;
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = expressionStatement });
                return new List<CodeExecution>() { executionPath };
            };
    }
}
