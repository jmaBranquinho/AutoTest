using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers
{
    public static class ExpressionHelper
    {
        public static (string MethodCalled, IEnumerable<ArgumentSyntax> arguments) GetMethodReferences(ExpressionSyntax expression)
        {
            var skippableTypes = new List<Type>
            {
                typeof(LiteralExpressionSyntax),
                typeof(IdentifierNameSyntax),
            };

            if(skippableTypes.Contains(expression.GetType())) 
            {
                return (null, null);
            }

            if(expression is InvocationExpressionSyntax invocationExpression)
            {
                var methodCalled = ((IdentifierNameSyntax) invocationExpression.Expression).Identifier.Value.ToString();
                var arguments = invocationExpression.ArgumentList.Arguments;
                return (methodCalled, arguments);
            }

            throw new NotImplementedException();
        }
    }
}
