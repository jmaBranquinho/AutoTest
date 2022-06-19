using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.Interfaces
{
    public interface ISyntaxAnalyzer
    {
        public Type? ReferredType { get; }

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, List<CodeExecution>>, List<CodeExecution>> Analyze { get; }
    }
}
