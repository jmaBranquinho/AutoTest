using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ReturnStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ReturnStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var returnStatement = (ReturnStatementSyntax)statement;
                executionPath.IsFinished = true;

                var result = new List<CodeExecution>() { executionPath };
                var isConditional = returnStatement?.Expression?.GetType() == typeof(ConditionalExpressionSyntax);
                SyntaxNode node = isConditional ? returnStatement?.Expression : returnStatement;
                var reference = ExpressionHelper.GetMethodReferences(returnStatement?.Expression);

                if (isConditional)
                {
                    var clone = executionPath.Clone();
                    result.Add(clone);
                }

                result.ForEach(path =>
                    path.Execution.Add(new StatementWrapper { SyntaxNode = node, Reference = reference }));

                return result;
            };
    }
}
