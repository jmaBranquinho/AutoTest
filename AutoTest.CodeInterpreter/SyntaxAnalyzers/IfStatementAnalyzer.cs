using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class IfStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(IfStatementSyntax);

        public Func<SyntaxNode, ExecutionPath, Func<SyntaxNode, ExecutionPath, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var ifSyntax = (IfStatementSyntax)statement;

                var clone = (ExecutionPath)executionPath.Clone();
                executionPath.Add(new StatementWrapper { SyntaxNode = ifSyntax });
                clone.Add(new StatementWrapper { SyntaxNode = ifSyntax, IsElseStatement = true });

                var results = recursiveFunction(ifSyntax.Statement, executionPath)
                    .Union(recursiveFunction(ifSyntax?.Else?.Statement, clone));

                return results;
            };
    }
}
