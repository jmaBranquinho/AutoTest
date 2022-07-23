using AutoTest.CodeInterpreter.Analyzers;
using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Analyzers;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Services
{
    /// <summary>
    /// Goes throught the execution paths of a method, trying to identify variables, constraints for parameters and variables and possible return value
    /// </summary>
    public class CodeRunnerService
    {
        public static IEnumerable<CodeRunExecution> RunMethod(MethodWrapper method) 
            => method.ExecutionPaths.Select(path =>
                new CodeRunExecution
                {
                    Method = method,
                    Path = path,
                    Parameters = GenerateParameters(method.Parameters, path),
                });

        private static IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> GenerateParameters(Dictionary<string, Type> parameters, IEnumerable<StatementWrapper> path)
        {
            var methodStatements = path.Skip(1).ToList();

            var constraints = new Dictionary<string, IConstraint>();
            PopulateParameterConstraints(constraints, parameters);

            methodStatements
                .ForEach(statement => AdjustParameterConstraints(statement, constraints));

            var parameterListWithValues = GenerateParameterListWithValues(parameters, constraints);

            return new List<List<(string Name, Type Type, object Value)>> { parameterListWithValues.ToList() };
        }

        // TODO: implement
        // TODO: extract
        private static void AdjustParameterConstraints(StatementWrapper statementWrapper, Dictionary<string, IConstraint> constraints)
        {
            switch (statementWrapper.SyntaxNode)
            {
                case IfStatementSyntax ifSyntax:
                    OperationsAnalyzer.AdjustConstraints(constraints, (BinaryExpressionSyntax)ifSyntax.Condition, statementWrapper.IsElseStatement);
                    break;
                case ForStatementSyntax:
                    throw new Exception("this should have been handled before - something's wrong");// TODO
                default:
                    break;
            }
        }

        private static IEnumerable<(string Name, Type Type, object Value)> GenerateParameterListWithValues(Dictionary<string, Type> parameters, Dictionary<string, IConstraint> constraints)
        {
            foreach (var parameter in parameters)
            {
                yield return (parameter.Key, parameter.Value, constraints[parameter.Key].Generate());
            }
        }

        // TODO: implement for other types
        private static void PopulateParameterConstraints(Dictionary<string, IConstraint> constraints, Dictionary<string, Type> parameters)
            => parameters.ToList()
                .ForEach(parameter => constraints.Add(parameter.Key, AnalyzerHelper.GetConstraintFromType(parameter.Value)));
    }
}
