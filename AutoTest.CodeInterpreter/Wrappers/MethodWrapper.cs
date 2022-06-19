using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class MethodWrapper
    {
        public string Name { get; set; }

        public IEnumerable<IEnumerable<StatementWrapper>> ExecutionPaths { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public void AnalyzeMethodDetails()
        {
            Parameters = new Dictionary<string, string>();

            var methodSyntax = GetMethodSyntax();
            // TODO test for null

            foreach (var parameter in methodSyntax.ParameterList.Parameters)
            {
                var typeAsString = ((PredefinedTypeSyntax)parameter.Type).Keyword.Text;
                var varName = parameter.Identifier.Text;
                Parameters.Add(varName, typeAsString);
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

            return null;
        }
    }
}
