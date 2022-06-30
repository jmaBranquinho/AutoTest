using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public class OperationsAnalyzer
    {

        public OperationsAnalyzer()
        {
            PopulateAnalyzerDictionary();
        }

        public static void AdjustConstraints(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, bool IsElseStatement)
        {
            var kind = binaryExpression.Kind();


        }

        private void PopulateAnalyzerDictionary()
        {
            //TODO implement

            var analyzers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IOperationsAnalyzer).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                .ToList();
            ;
        }
    }
}
