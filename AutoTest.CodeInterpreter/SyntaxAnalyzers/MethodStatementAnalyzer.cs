using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class MethodStatementAnalyzer : StatementAnalyzerBase, IRequestHandler<StatementAnalysisRequest<MethodDeclarationSyntax, MethodWrapper>, MethodWrapper>
    {
        public Task<MethodWrapper> Handle(StatementAnalysisRequest<MethodDeclarationSyntax, MethodWrapper> request, CancellationToken cancellationToken)
        {
            request.ExecutionPath.Add(new StatementWrapper { SyntaxNode = (MethodDeclarationSyntax)request.Statement });

            //var result = request.RecursiveFunction(((MethodDeclarationSyntax)request.Statement).Body, request.ExecutionPath);


            MethodInfo method = typeof(StatementAnalyzerBase).GetMethod("NewRequest", BindingFlags.NonPublic);
            method = method.MakeGenericMethod(((MethodDeclarationSyntax)request.Statement).Body.GetType(), typeof(IEnumerable<ExecutionPath>));
            var result = method.Invoke(this, new object[] { ((MethodDeclarationSyntax)request.Statement).Body, request.ExecutionPath });

            return Task.FromResult((MethodWrapper)result);
        }


    }
}
