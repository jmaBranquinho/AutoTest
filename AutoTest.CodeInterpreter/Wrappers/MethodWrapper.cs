using AutoTest.CodeInterpreter.Services;
using AutoTest.Core.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class MethodWrapper
    {
        public string Name { get; set; }

        public IEnumerable<IEnumerable<StatementWrapper>> ExecutionPaths { get; set; }

        public Dictionary<string, Type> Parameters { get; set; }

        public ClassWrapper Class { get; set; }

        private List<string> _references = new();

        public void AnalyzeMethodDetails()
        {
            Parameters = new Dictionary<string, Type>();

            var methodSyntax = GetMethodSyntax();

            methodSyntax.ParameterList.Parameters
                .ToList()
                .ForEach(parameter => Parameters
                    .Add(parameter.Identifier.Text, PrimitiveTypeConvertionHelper.GetTypeFromString(((PredefinedTypeSyntax)parameter.Type).Keyword.ValueText)));
        }

        public bool IsConsolidationRequired() => _references.Any() || ExecutionPaths.Any(p => p.Any(s => s.IsLoopStatement));

        public IEnumerable<string> GetReferences() => _references;

        public void Consolidate(SolutionWrapper solution, ConsolidationService consolidationService)
        {
            _references.AddRange(
                ExecutionPaths
                .SelectMany(path => path
                    .Where(statement => statement.HasReference)
                    .Select(statement => statement.Reference.MethodCalled)));

            consolidationService.RegisterMethod(this);
        }

        private MethodDeclarationSyntax GetMethodSyntax()
        {
            if (ExecutionPaths is not null && ExecutionPaths.Any())
            {
                ExecutionPaths = ExecutionPaths.Where(x => x is not null && x.Any()).ToList();

                var methodDeclarationSyntax = ExecutionPaths
                    .SelectMany(path => path
                        .Where(statement => statement.SyntaxNode.GetType() == typeof(MethodDeclarationSyntax))
                        .Select(statement => statement))
                    .FirstOrDefault();

                if (methodDeclarationSyntax is not null)
                {
                    return (MethodDeclarationSyntax)methodDeclarationSyntax.SyntaxNode;
                }
            }

            throw new Exception("Empty execution path");//TODO
        }
    }
}
