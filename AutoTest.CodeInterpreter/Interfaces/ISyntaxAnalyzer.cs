using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.Interfaces
{
    public interface ISyntaxAnalyzer
    {
        public Type? ReferredType { get; }

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze { get; }
    }
}
