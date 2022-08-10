using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ExpressionStatementAnalyzer : IRequestHandler<StatementAnalysisRequest<ExpressionStatementSyntax, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>>
    {
        public Task<IEnumerable<ExecutionPath>> Handle(StatementAnalysisRequest<ExpressionStatementSyntax, IEnumerable<ExecutionPath>> request, CancellationToken cancellationToken)
        {
            request.ExecutionPath.Add(new StatementWrapper { SyntaxNode = (ExpressionStatementSyntax)request.Statement });
            var result = new List<ExecutionPath>() { request.ExecutionPath };
            return Task.FromResult((IEnumerable<ExecutionPath>)result);
        }
    }
}
