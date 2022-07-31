using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class FallbackStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => null;

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                throw new NotImplementedException();
            };
    }
}
