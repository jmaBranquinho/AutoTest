﻿using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers
{
    public class LocalDeclarationStatementAnalyzer : ISyntaxAnalyzer
    {
        public Type? ReferredType => typeof(LocalDeclarationStatementSyntax);

        public Func<SyntaxNode, CodeExecution, Func<List<SyntaxNode>, CodeExecution, List<CodeExecution>>, List<CodeExecution>> Analyze =>
            (statement, executionPath, recursiveFunction) =>
            {
                executionPath.Execution.Add(new StatementWrapper { SyntaxNode = statement });
                return recursiveFunction(new List<SyntaxNode> { }, executionPath);
            };
    }
}
