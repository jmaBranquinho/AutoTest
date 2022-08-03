using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class IfStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(IfStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var ifSyntax = (IfStatementSyntax)statement;

                var clone = executionPath.Clone();
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = ifSyntax });
                clone.Execution.Add(new StatementWrapper { SyntaxNode = ifSyntax, IsElseStatement = true });

                var results = recursiveFunction(ifSyntax.Statement, executionPath)
                    .Union(recursiveFunction(ifSyntax?.Else?.Statement, clone));

                return results;
            };
    }
}
