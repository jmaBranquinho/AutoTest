using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class IfStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(IfStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var ifSyntax = (IfStatementSyntax)statement;

                var clone = executionPath.Clone();
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = ifSyntax });
                clone.Execution.Add(new StatementWrapper { SyntaxNode = ifSyntax, IsElseStatement = true });

                var results = new List<CodeExecution>();
                results.AddRange(recursiveFunction(new List<SyntaxNode> { ifSyntax.Statement }, executionPath));
                results.AddRange(recursiveFunction(new List<SyntaxNode> { ifSyntax?.Else?.Statement }, clone));
                return results;
            };
    }
}
