using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class MethodStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(MethodDeclarationSyntax);

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, List<CodeExecution>>, List<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = (MethodDeclarationSyntax)statement });
                return recursiveFunction(new List<SyntaxNode>() { ((MethodDeclarationSyntax)statement).Body }, executionPath);
            };
    }
}
