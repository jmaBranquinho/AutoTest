using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class StatementWrapper
    {
        public SyntaxNode SyntaxNode { get; set; }

        public bool IsElseStatement { get; set; }

        public (string MethodCalled, IEnumerable<ArgumentSyntax> Arguments) Reference { get; set; }

        public bool HasReference => !string.IsNullOrWhiteSpace(Reference.MethodCalled);
    }
}
