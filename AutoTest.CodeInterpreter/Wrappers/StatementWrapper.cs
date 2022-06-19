using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class StatementWrapper
    {
        public SyntaxNode SyntaxNode { get; set; }

        public bool IsElseStatement { get; set; }
    }
}
