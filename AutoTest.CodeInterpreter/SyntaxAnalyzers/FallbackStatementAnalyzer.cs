using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class FallbackStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => null;

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze => throw new NotImplementedException();
    }
}
