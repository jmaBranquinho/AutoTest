using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class BlockStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(BlockSyntax);

        public Func<SyntaxNode, ExecutionPath, Func<SyntaxNode, ExecutionPath, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>> Analyze =>
            (statement, executionPath, recursiveFunction) 
                => ((BlockSyntax)statement).Statements.Cast<SyntaxNode>().ToList()
                    .Aggregate(new List<ExecutionPath> { executionPath }, (paths, statement)
                        => paths.SelectMany(path => recursiveFunction(statement, path)).ToList());
    }
}
