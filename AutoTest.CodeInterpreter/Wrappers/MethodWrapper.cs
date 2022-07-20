using AutoTest.CodeInterpreter.Consolidation;
using AutoTest.Core;
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

            foreach (var parameter in methodSyntax.ParameterList.Parameters)
            {
                Parameters.Add(parameter.Identifier.Text, PrimitiveTypeConvertionHelper.GetTypeFromString(((PredefinedTypeSyntax) parameter.Type).Keyword.ValueText));
            }
        }

        public bool IsConsolidationRequired() => _references.Any();

        public IEnumerable<string> GetReferences() => _references;

        public void Consolidate(SolutionWrapper solution, ConsolidationService consolidationService)
        {
            foreach (var path in ExecutionPaths)
            {
                foreach (var statement in path)
                {
                    if(statement.HasReference)
                    {
                        _references.Add(statement.Reference.MethodCalled);
                    }
                }
            }

            consolidationService.RegisterMethod(this);
        }

        private MethodDeclarationSyntax GetMethodSyntax()
        {
            if (ExecutionPaths is not null)
            {
                if(ExecutionPaths.Any(x => x is null || !x.Any()))
                {
                    ExecutionPaths = ExecutionPaths.Where(x => x is not null && x.Any()).ToList();
                }

                foreach (var path in ExecutionPaths)
                {
                    foreach (var statement in path)
                    {
                        if (statement.SyntaxNode.GetType() == typeof(MethodDeclarationSyntax))
                        {
                            return (MethodDeclarationSyntax)statement.SyntaxNode;
                        }
                    }
                }
            }

            throw new Exception("Empty execution path");//TODO
        }
    }
}
