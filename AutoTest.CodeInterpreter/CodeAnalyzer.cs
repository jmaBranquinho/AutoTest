using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.CodeInterpreter.SyntaxAnalyzers;
using AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTest.CodeInterpreter
{
    // TODO: fails if no namespace is provided
    public class CodeAnalyzer
    {
        private readonly SyntaxAnalyzerDictionary _dictionary;
        private readonly ISender _mediator;

        public CodeAnalyzer()
        {
            _dictionary = new SyntaxAnalyzerDictionary();
            var serviceProvider = new ServiceCollection()
                .AddMediatR(typeof(CodeAnalyzer).GetType().Assembly)
                .BuildServiceProvider();
            _mediator = serviceProvider.GetRequiredService<ISender>();
        }

        public SolutionWrapper AnalyzeCode(string code) => PerformCodeAnalysis(code);

        public SolutionWrapper AnalyzeCodeFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("wrong path?");
            }
            var code = File.ReadAllText(filePath);

            return PerformCodeAnalysis(code);
        }

        private SolutionWrapper PerformCodeAnalysis(string code) 
            => new SolutionWrapper
                {
                    Namespaces = GetNamespaces(code),
                }
                .Consolidate();

        private IEnumerable<NamespaceWrapper> GetNamespaces(string code)
            => GetCompilationUnitSyntax(code)
                .Members
                .Cast<SyntaxNode>()
                .Select(namespaceStatement => new NamespaceWrapper
                {
                    Name = ((NamespaceDeclarationSyntax)namespaceStatement).Name.ToFullString(),
                    Classes = GetClasses(namespaceStatement),
                })
                .ToList();

        private IEnumerable<ClassWrapper> GetClasses(SyntaxNode namespaceStatement)
            => namespaceStatement
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Cast<SyntaxNode>()
                .Select(classStatement => new ClassWrapper
                {
                    Name = ((ClassDeclarationSyntax)classStatement).Identifier.ValueText,
                    Methods = GetMethods2(classStatement)
                })
            .ToList();

        private IEnumerable<MethodWrapper> GetMethods2(SyntaxNode classStatement)
            => classStatement
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(methodStatement => GetExecutionPaths2(methodStatement, new ExecutionPath()))
                .ToList();

        private MethodWrapper GetExecutionPaths2(MethodDeclarationSyntax methodStatement, ExecutionPath executionPath)
        {
            return _mediator.Send(new StatementAnalysisRequest<StatementAnalysisRequest<MethodDeclarationSyntax, MethodWrapper>, MethodWrapper>
            {
                Statement = methodStatement,
                ExecutionPath = executionPath,
            }).GetAwaiter().GetResult();
        }

        //private IEnumerable<MethodWrapper> GetMethods(SyntaxNode classStatement)
        //    => classStatement
        //        .DescendantNodes()
        //        .OfType<MethodDeclarationSyntax>()
        //        .Select(methodStatement => new MethodWrapper
        //        {
        //            Name = methodStatement.Identifier.ValueText,
        //            ExecutionPaths = GetExecutionPaths2(methodStatement, new ExecutionPath()).ToList(),
        //        })
        //        .ToList();

        //private IEnumerable<ExecutionPath> GetExecutionPaths(SyntaxNode statement, ExecutionPath currentExecutionPath) 
        //    => statement == null
        //        ? new List<ExecutionPath> { currentExecutionPath }
        //        : _dictionary
        //            .GetAnalyzerFromDictionary(statement.GetType())
        //            .Analyze(statement, currentExecutionPath, GetExecutionPaths);



        private static CompilationUnitSyntax GetCompilationUnitSyntax(string code) 
            => CSharpSyntaxTree.ParseText(code).GetCompilationUnitRoot();
    }
}
