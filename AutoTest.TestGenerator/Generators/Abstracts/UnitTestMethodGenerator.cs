﻿using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Analyzers;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestMethodGenerator : ITestMethodGenerator
    {
        public IEnumerable<UnitTest> GenerateUnitTests(MethodWrapper method, TestNamingConventions namingConvention)
        {
            Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, UnitTest> createUnitTest = GenerateUnitTest(method);

            return method.ExecutionPaths.Select(path => 
                createUnitTest(FormatMethodName(method.Name, namingConvention), GenerateParameters(method.Parameters, path), path))
                    .ToList();
        }

        protected abstract Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, UnitTest> GenerateUnitTest(MethodWrapper method);

        // TODO: implement the rest
        private IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> GenerateParameters(Dictionary<string, Type> parameters, IEnumerable<StatementWrapper> path)
        {
            var methodStatements = path.Skip(1).ToList();

            var constraints = new Dictionary<string, IConstraint>();
            PopulateParameterConstraints(constraints, parameters);

            foreach (var statement in methodStatements)
            {
                AdjustParameterConstraints(statement, constraints);
            }

            var parameterListWithValues = GenerateParameterListWithValues(parameters, constraints);

            return new List<List<(string Name, Type Type, object Value)>> { parameterListWithValues.ToList() };
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
        {
            foreach (var parameter in parameters)
            {
                constraints.Add(parameter.Key, AnalyzerHelper.GetConstraintFromType(parameter.Value));
            }
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

        // TODO: implement
        private static string FormatMethodName(string methodName, TestNamingConventions namingConvention)
        {
            return $"{methodName}_WhenSomething_ShouldSomething";
        }
    }
}
