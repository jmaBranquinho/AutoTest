//using AutoTest.CodeInterpreter.Interfaces;
//using AutoTest.CodeInterpreter.Models.Wrappers;
//using MediatR;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;

//namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
//{
//    public class LocalDeclarationStatementAnalyzer : IRequestHandler<StatementAnalysisRequest<LocalDeclarationStatementSyntax, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>>
//    {
//        public Task<IEnumerable<ExecutionPath>> Handle(StatementAnalysisRequest<LocalDeclarationStatementSyntax, IEnumerable<ExecutionPath>> request, CancellationToken cancellationToken)
//        {
//            request.ExecutionPath.Add(new StatementWrapper { SyntaxNode = request.Statement });
//            var result = request.RecursiveFunction(null, request.ExecutionPath);
//            return Task.FromResult(result);
//        }
//    }
//}
