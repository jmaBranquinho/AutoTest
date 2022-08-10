//using AutoTest.CodeInterpreter.Interfaces;
//using AutoTest.CodeInterpreter.Models.Wrappers;
//using MediatR;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;

//namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
//{
//    public class BlockStatementAnalyzer : IRequestHandler<StatementAnalysisRequest<BlockSyntax, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>>
//    {
//        public Task<IEnumerable<ExecutionPath>> Handle(StatementAnalysisRequest<BlockSyntax, IEnumerable<ExecutionPath>> request, CancellationToken cancellationToken)
//        {
//            var result = ((BlockSyntax)request.Statement).Statements.Cast<SyntaxNode>().ToList()
//                    .Aggregate(new List<ExecutionPath> { request.ExecutionPath }, (paths, statement)
//                        => paths.SelectMany(path => request.RecursiveFunction(statement, path)).ToList());

//            return Task.FromResult((IEnumerable<ExecutionPath>)result);
//        }
//    }
//}
