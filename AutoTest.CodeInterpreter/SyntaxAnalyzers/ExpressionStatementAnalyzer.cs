﻿using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class ExpressionStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(ExpressionStatementSyntax);

        public Func<SyntaxNode, ExecutionPath, Func<SyntaxNode, ExecutionPath, IEnumerable<ExecutionPath>>, IEnumerable<ExecutionPath>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Add(new StatementWrapper { SyntaxNode = (ExpressionStatementSyntax)statement });
                return new List<ExecutionPath>() { executionPath };
            };
    }
}
