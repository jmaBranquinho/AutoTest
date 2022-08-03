using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ForStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ForStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = statement, IsLoopStatement = true });
                return recursiveFunction(null, executionPath);
            };
    }
}
