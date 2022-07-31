using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class BlockStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(BlockSyntax);

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                return recursiveFunction(((BlockSyntax)statement).Statements.Cast<SyntaxNode>().ToList(), executionPath);
            };
    }
}
