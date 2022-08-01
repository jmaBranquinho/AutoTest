using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class BlockStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(BlockSyntax);

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) 
                => ((BlockSyntax)statement).Statements.Cast<SyntaxNode>().ToList()
                    .Aggregate(new List<CodeExecution> { executionPath }, (paths, statement)
                        => paths.SelectMany(path => recursiveFunction(statement, path)).ToList());
    }
}
