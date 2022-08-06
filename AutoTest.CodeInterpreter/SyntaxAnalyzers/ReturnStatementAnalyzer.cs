using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ReturnStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ReturnStatementSyntax);

        public Func<SyntaxNode, ExecutionPath, Func<SyntaxNode, ExecutionPath, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var returnStatement = (ReturnStatementSyntax)statement;

                var result = new List<ExecutionPath>() { executionPath };
                var isConditional = returnStatement?.Expression?.GetType() == typeof(ConditionalExpressionSyntax);
                SyntaxNode node = isConditional ? returnStatement?.Expression : returnStatement;
                var reference = ExpressionHelper.GetMethodReferences(returnStatement?.Expression);

                if (isConditional)
                {
                    var clone = (ExecutionPath)executionPath.Clone();
                    result.Add(clone);
                }

                result.ForEach(path =>
                    path.Add(new StatementWrapper { SyntaxNode = node, Reference = reference }));

                return result;
            };
    }
}
