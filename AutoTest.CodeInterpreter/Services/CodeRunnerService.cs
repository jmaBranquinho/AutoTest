using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Analyzers;
using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.Core.Helpers;
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
        public IEnumerable<CodeRunExecution> RunMethod(MethodWrapper method) 
            => method.ExecutionPaths.Select(path 
                => IterateMethodStatements(method, path));

        private static CodeRunExecution IterateMethodStatements(MethodWrapper method, IEnumerable<StatementWrapper> path)
        {
            var methodStatements = path.Skip(1).ToList();

            var parameterConstraints = new Dictionary<string, IConstraint>();
            PopulateParameterConstraints(parameterConstraints, method.Parameters);
            var returnParameter = GetMethodReturnType(path.First());

            methodStatements.ForEach(statement => OperationsAnalyzerHelper.AdjustParameterConstraints(returnParameter, parameterConstraints, statement));

            return new CodeRunExecution
            {
                Method = method,
                Path = path,
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
                .ForEach(parameter => constraints.Add(parameter.Key, NumericHelper.GetConstraintFromType(parameter.Value)));
    }
}
