using AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers;
using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter
{
    public class CodeAnalyzer
    {
        private readonly SyntaxAnalyzerDictionary _dictionary;

        public CodeAnalyzer()
        {
            _dictionary = new SyntaxAnalyzerDictionary();
        }

        public SolutionWrapper Analyze(string path)
        {
            var root = GetCompilationUnitSyntax(path);
            var namespaceStatements = root.Members.Cast<SyntaxNode>().ToList();

            var solutionWrapper = new SolutionWrapper
            {
                Name = "Solution", // TODO: change name
                Namespaces = new List<NamespaceWrapper>(),
            };

            foreach (var namespaceStatement in namespaceStatements)
            {
                var namespaceWrapper = new NamespaceWrapper
                {
                    Name = ((NamespaceDeclarationSyntax)namespaceStatement).Name.ToFullString(),
                    Classes = new List<ClassWrapper>(),
                };
                solutionWrapper.Namespaces.Add(namespaceWrapper);

                var classStatements = namespaceStatement.DescendantNodes().OfType<ClassDeclarationSyntax>().Cast<SyntaxNode>().ToList();

                foreach (var classStatement in classStatements)
                {
                    var classWrapper = new ClassWrapper
                    {
                        Name = ((ClassDeclarationSyntax)classStatement).Identifier.ValueText,
                        Methods = new List<MethodWrapper>(),
                    };
                    namespaceWrapper.Classes.Add(classWrapper);

                    var methods = classStatement.DescendantNodes().OfType<MethodDeclarationSyntax>();

                    var publicMethods = methods.Where(m => m.Modifiers.Any(mod => mod.Text == "public")).ToList();

                    foreach (var methodStatement in publicMethods)
                    {
                        var methodWrapper = new MethodWrapper
                        {
                            Name = methodStatement.Identifier.ValueText,
                            ExecutionPaths = TryHandleStatement(new List<SyntaxNode> { methodStatement }, new CodeExecution()).Select(x => x.Execution).ToList(),
                        };
                        methodWrapper.AnalyzeMethodDetails();
                        classWrapper.Methods.Add(methodWrapper);
                    }
                }
            }
            return solutionWrapper;
        }

        private List<CodeExecution> TryHandleStatement(List<SyntaxNode> statements, CodeExecution currentExecutionPaths)
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
                return analyzer.Analyze(statement, currentExecutionPaths, TryHandleStatement);
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
                    var analyzisResults = TryHandleStatement(list, executionPath);
                    results.AddRange(analyzisResults.Where(path => path.IsFinished).ToList());
                    nextExecutionPaths.AddRange(analyzisResults);
                }

                executionPaths = nextExecutionPaths;
            }

            return executionPaths;
        }

        private CompilationUnitSyntax GetCompilationUnitSyntax(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("wrong path?");
            }
            var code = File.ReadAllText(path);

            var tree = CSharpSyntaxTree.ParseText(code);
            return tree.GetCompilationUnitRoot();
        }
    }
}
