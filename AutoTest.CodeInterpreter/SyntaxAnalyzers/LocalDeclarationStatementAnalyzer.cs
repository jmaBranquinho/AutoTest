using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class LocalDeclarationStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(LocalDeclarationStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = statement });
                return recursiveFunction(null, executionPath);
            };
    }
}
