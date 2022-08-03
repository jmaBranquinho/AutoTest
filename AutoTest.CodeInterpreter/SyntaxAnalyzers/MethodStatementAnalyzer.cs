﻿using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Models.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class MethodStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(MethodDeclarationSyntax);

        public Func<SyntaxNode, CodeExecution, Func<SyntaxNode, CodeExecution, IEnumerable<CodeExecution>>, IEnumerable<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = (MethodDeclarationSyntax)statement });
                return recursiveFunction(((MethodDeclarationSyntax)statement).Body, executionPath);
            };
    }
}
