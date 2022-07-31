using AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter
{
    // TODO: fails if no namespace is provided
    public class CodeAnalyzer
    {
        private readonly SyntaxAnalyzerDictionary _dictionary;

        public CodeAnalyzer() => _dictionary = new SyntaxAnalyzerDictionary();

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
                    Methods = GetMethods(classStatement)
                })
            .ToList();

        private IEnumerable<MethodWrapper> GetMethods(SyntaxNode classStatement)
            => classStatement
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(methodStatement => new MethodWrapper
                {
                    Name = methodStatement.Identifier.ValueText,
                    ExecutionPaths = GetExecutionPaths(new List<SyntaxNode> { methodStatement }, new CodeExecution()).Select(x => x.Execution).ToList(),
                })
                .ToList();

        private IEnumerable<CodeExecution> GetExecutionPaths(List<SyntaxNode> statements, CodeExecution currentExecutionPaths)
        {
            if (!statements.Any() || statements.First() == null || currentExecutionPaths.IsFinished)
            {
                return new List<CodeExecution>() { currentExecutionPaths };
            }

            var executionPaths = new List<CodeExecution> {currentExecutionPaths};

            statements.ForEach(statement =>
            {
                executionPaths = executionPaths
                    .SelectMany(executionPath =>
                    {
                        var analyzer = _dictionary.GetAnalyzerFromDictionary(statement.GetType());
                        return analyzer.Analyze(statement, currentExecutionPaths, GetExecutionPaths);
                    })
                    .ToList();
            });

            return executionPaths;
        }

        private static CompilationUnitSyntax GetCompilationUnitSyntax(string code) 
            => CSharpSyntaxTree.ParseText(code).GetCompilationUnitRoot();
    }
}
