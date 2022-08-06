using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.CodeInterpreter.OperationAnalyzers;
using AutoTest.CodeInterpreter.OperationAnalyzers.Helpers;
using AutoTest.Core.Helpers;
using AutoTest.Core.Models;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Services
{
    /// <summary>
    /// Goes throught the execution paths of a method, trying to identify variables, constraints for parameters and variables and possible return value
    /// </summary>
    public class CodeRunnerService
    {
        public IEnumerable<ExecutionPathInfo> RunMethod(MethodWrapper method) 
            => method.ExecutionPaths.Select(path 
                => IterateMethodStatements(method, path));

        private static ExecutionPathInfo IterateMethodStatements(MethodWrapper method, ExecutionPath path)
        {
            var methodStatements = path.Skip(1).ToList();

            var parameterConstraints = new Dictionary<string, IConstraint>();
            PopulateParameterConstraints(parameterConstraints, method.Parameters);
            var returnParameter = GetMethodReturnType(path.First());

            methodStatements.ForEach(statement => OperationsAnalyzerHelper.AdjustParameterConstraints(returnParameter, parameterConstraints, statement));

            return new ExecutionPathInfo(path)
            {
                Method = method,
                ParameterConstraints = parameterConstraints,
                ReturnParameter = returnParameter,
            };
        }

        private static LiteralOrParameterDefinition GetMethodReturnType(StatementWrapper statementWrapper)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax)statementWrapper.SyntaxNode;
            var returnTypeAsString = ((PredefinedTypeSyntax)methodDeclarationSyntax.ReturnType).Keyword.ValueText;
            return new LiteralOrParameterDefinition { Type = PrimitiveTypeConvertionHelper.GetTypeFromString(returnTypeAsString) };
        }

        private static void PopulateParameterConstraints(Dictionary<string, IConstraint> constraints, Dictionary<string, Type> parameters)
            => parameters.ToList()
                .ForEach(parameter => constraints.Add(parameter.Key, NumericOperationHelper.GetConstraintFromType(parameter.Value)));
    }
}
