//using AutoTest.CodeInterpreter.Interfaces;
//using AutoTest.CodeInterpreter.Models.Wrappers;
//using MediatR;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;

//namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
//{
//    public class IfStatementAnalyzer : IRequestHandler<StatementAnalysisRequest<IfStatementSyntax, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>>
//    {
//        public Task<IEnumerable<ExecutionPath>> Handle(StatementAnalysisRequest<IfStatementSyntax, IEnumerable<ExecutionPath>> request, CancellationToken cancellationToken)
//        {
//            var ifSyntax = (IfStatementSyntax)request.Statement;

//            var clone = request.ExecutionPath.Clone();
//            request.ExecutionPath.Add(new StatementWrapper { SyntaxNode = ifSyntax });
//            clone.Add(new StatementWrapper { SyntaxNode = ifSyntax, IsElseStatement = true });

//            var results = request.RecursiveFunction(ifSyntax.Statement, request.ExecutionPath)
//                .Union(request.RecursiveFunction(ifSyntax?.Else?.Statement, clone));

//            return Task.FromResult(results);
//        }
//    }
//}
