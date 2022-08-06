using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.Interfaces
{
    public interface ISyntaxAnalyzer
    {
        public Type? ReferredType { get; }

        public Func<SyntaxNode, ExecutionPath, Func<SyntaxNode, ExecutionPath, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>> Analyze { get; }
    }
}
