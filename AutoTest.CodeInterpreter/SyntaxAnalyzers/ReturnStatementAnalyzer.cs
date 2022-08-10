using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ReturnStatementAnalyzer : IRequestHandler<StatementAnalysisRequest<ReturnStatementSyntax, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>>
    {
        public Task<IEnumerable<ExecutionPath>> Handle(StatementAnalysisRequest<ReturnStatementSyntax, IEnumerable<ExecutionPath>> request, CancellationToken cancellationToken)
        {
            var returnStatement = (ReturnStatementSyntax)request.Statement;

            var result = new List<ExecutionPath>() { request.ExecutionPath };
            var isConditional = returnStatement?.Expression?.GetType() == typeof(ConditionalExpressionSyntax);
            SyntaxNode node = isConditional ? returnStatement?.Expression : returnStatement;
            var reference = ExpressionHelper.GetMethodReferences(returnStatement?.Expression);

            if (isConditional)
            {
                var clone = request.ExecutionPath.Clone();
                result.Add(clone);
            }

            result.ForEach(path =>
                path.Add(new StatementWrapper { SyntaxNode = node, Reference = reference }));

            return Task.FromResult((IEnumerable<ExecutionPath>)result);
        }
    }
}
