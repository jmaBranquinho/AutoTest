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
            // TODO test for null

            foreach (var parameter in methodSyntax.ParameterList.Parameters)
            {
                Parameters.Add(parameter.Identifier.Text, PrimitiveTypeConvertionHelper.GetTypeFromString(((PredefinedTypeSyntax) parameter.Type).Keyword.ValueText));
            }
        }

        private MethodDeclarationSyntax GetMethodSyntax()
        {
            if (ExecutionPaths is not null)
            {
                foreach (var path in ExecutionPaths)
                {
                    if (path is null)
                    {
                        continue;
                    }

                    foreach (var statement in path)
                    {
                        if (statement.SyntaxNode.GetType() == typeof(MethodDeclarationSyntax))
                        {
                            return (MethodDeclarationSyntax)statement.SyntaxNode;
                        }
                    }
                }
            }

            throw new NotImplementedException();//TODO
        }
    }
}
