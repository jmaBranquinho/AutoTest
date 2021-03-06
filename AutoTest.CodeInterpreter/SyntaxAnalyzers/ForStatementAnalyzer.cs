using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ForStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ForStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, List<CodeExecution>>, List<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var forStatementChild = ((ForStatementSyntax)statement).Statement;

                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = statement, IsLoopStatement = true });
                return recursiveFunction(new List<SyntaxNode>(), executionPath);
            };
    }
}
