using AutoTest.CodeInterpreter.Models.Wrappers;
using MediatR;
using Microsoft.CodeAnalysis;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class StatementAnalysisRequest<TInput, TOutput> : IRequest<TOutput>
    {
        public SyntaxNode Statement { get; set; }

        public ExecutionPath ExecutionPath { get; set; }

        //public TInput ReferredType { get; set; }

        //public TOutput ReturnedType { get; set; }

        public ISender Mediator { get; set; }

        //public Func<SyntaxNode, ExecutionPath, TOutput> RecursiveFunction { get; set; }

        //public StatementAnalysisRequest(
        //    SyntaxNode statement,
        //    ExecutionPath executionPath,
        //    TInput referredType,
        //    Func<SyntaxNode, ExecutionPath, TOutput> recursiveFunction)
        //{
        //    Statement = statement;
        //    ExecutionPath = executionPath;
        //    ReferredType = referredType;
        //    RecursiveFunction = recursiveFunction;
        //}
    }
}
