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

        private List<CodeExecution> GetExecutionPaths(List<SyntaxNode> statements, CodeExecution currentExecutionPaths)
        {
            bool hasPathReachedEnd = statements == null ||
                !statements.Any() || statements.First() == null ||
                currentExecutionPaths.IsFinished;

            if (hasPathReachedEnd)
            {
                return new List<CodeExecution>() { currentExecutionPaths };
            }

            bool hasOnlyOnePathAvailable = statements!.Count == 1;

            if (hasOnlyOnePathAvailable)
            {
                var statement = statements.First();
                var analyzer = _dictionary.GetAnalyzerFromDictionary(statement.GetType());
                return analyzer.Analyze(statement, currentExecutionPaths, GetExecutionPaths);
            }

            var executionPaths = new List<CodeExecution>
            {
                currentExecutionPaths ?? new CodeExecution()
            };

            var results = new List<CodeExecution>();

            foreach (var statement in statements)
            {
                var nextExecutionPaths = new List<CodeExecution>();

                foreach (var executionPath in executionPaths.ToList())
                {
                    var list = new List<SyntaxNode>();
                    list.Add(statement);
                    var analyzisResults = GetExecutionPaths(list, executionPath);
                    results.AddRange(analyzisResults.Where(path => path.IsFinished).ToList());
                    nextExecutionPaths.AddRange(analyzisResults);
                }

                executionPaths = nextExecutionPaths;
            }

            return executionPaths;
        }

        private static CompilationUnitSyntax GetCompilationUnitSyntax(string code) 
            => CSharpSyntaxTree.ParseText(code).GetCompilationUnitRoot();
    }
}
