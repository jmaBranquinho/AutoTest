using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class LocalDeclarationStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(LocalDeclarationStatementSyntax);

        public Func<SyntaxNode, ExecutionPath, Func<SyntaxNode, ExecutionPath, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Add(new StatementWrapper { SyntaxNode = statement });
                return recursiveFunction(null, executionPath);
            };
    }
}
