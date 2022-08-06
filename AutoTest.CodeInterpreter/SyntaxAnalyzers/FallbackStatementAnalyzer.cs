using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class FallbackStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => null;

        public Func<SyntaxNode, ExecutionPath, Func<SyntaxNode, ExecutionPath, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>> Analyze => throw new NotImplementedException();
    }
}
