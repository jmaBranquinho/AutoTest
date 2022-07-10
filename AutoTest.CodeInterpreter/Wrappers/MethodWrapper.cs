using AutoTest.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class MethodWrapper
    {
        public string Name { get; set; }

        public IEnumerable<IEnumerable<StatementWrapper>> ExecutionPaths { get; set; }

        public Dictionary<string, Type> Parameters { get; set; }

        public void AnalyzeMethodDetails()
        {
            Parameters = new Dictionary<string, Type>();

            var methodSyntax = GetMethodSyntax();

            foreach (var parameter in methodSyntax.ParameterList.Parameters)
            {
                Parameters.Add(parameter.Identifier.Text, PrimitiveTypeConvertionHelper.GetTypeFromString(((PredefinedTypeSyntax) parameter.Type).Keyword.ValueText));
            }
        }

        public void Consolidate(SolutionWrapper solution)
        {
            foreach (var path in ExecutionPaths)
            {
                foreach (var statement in path)
                {
                    if(statement.HasReference)
                    {
                        // TODO: 

                        throw new NotImplementedException();
                    }
                }
            }
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
