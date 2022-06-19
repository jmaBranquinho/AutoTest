using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ReturnStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ReturnStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, List<CodeExecution>>, List<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                var returnStatement = (ReturnStatementSyntax)statement;

                if (returnStatement?.Expression?.GetType() == typeof(ConditionalExpressionSyntax))
                {
                    executionPath.IsFinished = true;
                    var clone = executionPath.Clone();

                    executionPath.Execution.Add(new StatementWrapper { SyntaxNode = returnStatement.Expression });
                    clone.Execution.Add(new StatementWrapper { SyntaxNode = returnStatement.Expression });

                    return new List<CodeExecution>() { executionPath, clone };
                }
                else
                {
                    executionPath.Execution.Add(new StatementWrapper { SyntaxNode = returnStatement });
                    executionPath.IsFinished = true;
                    return new List<CodeExecution>() { executionPath };
                }
            };
    }
}
