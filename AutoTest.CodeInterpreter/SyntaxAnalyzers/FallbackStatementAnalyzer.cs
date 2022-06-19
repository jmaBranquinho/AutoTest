
using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class FallbackStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => null;

        Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, List<CodeExecution>>, List<CodeExecution>> ISyntaxAnalyzer.Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                throw new NotImplementedException();
            };
    }
}
