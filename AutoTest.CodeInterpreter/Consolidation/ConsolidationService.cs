using AutoTest.CodeInterpreter.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.CodeInterpreter.Consolidation
{
    public class ConsolidationService
    {
        private HashSet<(MethodWrapper Method, bool IsConsolidated)> _methods = new();

        public void RegisterMethod(MethodWrapper method) => _methods.Add((method, !method.IsConsolidationRequired()));

        public void Consolidate() => ResolveMethodDependencies();

        private void ResolveMethodDependencies()
        {
            var consolidatedMethods = _methods.Where(m => m.IsConsolidated).ToList();
            var nonConsolidatedMethod = _methods
                .Except(consolidatedMethods)
                .OrderBy(x => x.Method.GetReferences().Count())
                .ToList();

            while (nonConsolidatedMethod.Count != 0)
            {
                var newlyConsolidatedMethods = new List<MethodWrapper>();
                foreach (var (method, isConsolidated) in nonConsolidatedMethod)
                {
                    var hasReferencesConsolidated = method.GetReferences().All(m1 => consolidatedMethods.Any(m2 => m2.Method.Name == m1));
                    var hasLoops = method.ExecutionPaths.Any(p => p.Any(s => s.IsLoopStatement));
                    if (hasReferencesConsolidated || hasLoops)
                    {
                        // TODO: look out for circular dependencies
                        ExtractDependenciesIntoExecutionPaths(consolidatedMethods, method);
                        newlyConsolidatedMethods.Add(method);
                    }
                }
                consolidatedMethods.AddRange(newlyConsolidatedMethods.Select(m => (m, true)));
                nonConsolidatedMethod.RemoveAll(m => newlyConsolidatedMethods.Contains(m.Method));
            }
            _methods = new HashSet<(MethodWrapper Method, bool IsConsolidated)>(consolidatedMethods);
        }

        private static void ExtractDependenciesIntoExecutionPaths(List<(MethodWrapper Method, bool IsConsolidated)> consolidatedMethods, MethodWrapper method)
        {
            var redesignedPaths = new List<List<StatementWrapper>>();
            foreach (var path in method.ExecutionPaths)
            {
                var currentPaths = new List<List<StatementWrapper>>();
                currentPaths.Add(new List<StatementWrapper>());

                foreach (var statement in path)
                {
                    if (statement.IsLoopStatement)
                    {
                        ExtractLoopLogicIntoExecutionPaths(currentPaths, statement);
                    }
                    else if (statement.HasReference)
                    {
                        var methodCalled = GetMethodCalled(consolidatedMethods, statement);
                        ReplaceStatementByMethodStatements(currentPaths, methodCalled.ExecutionPaths);
                    }
                    else
                    {
                        AddToAll(currentPaths, statement);
                    }
                }
                redesignedPaths.AddRange(currentPaths);
            }
            method.ExecutionPaths = redesignedPaths;
        }

        private static void ExtractLoopLogicIntoExecutionPaths(List<List<StatementWrapper>> paths, StatementWrapper statement)
        {
            //TODO: only working for FOR loops
            if(statement.SyntaxNode is ForStatementSyntax)
            {
                var forSyntax = ((ForStatementSyntax)statement.SyntaxNode);
                (_, var loops) = HandleForLoopLogic(forSyntax);
                //TODO replace var if used in logic
                //TODO check if there are sub-loops, ifs, etc.

                var forLoopBody = (BlockSyntax)forSyntax.Statement;
                foreach (var path in paths)
                {
                    Enumerable.Range(0, loops).ToList()
                        .ForEach(_ => path.AddRange(
                            forLoopBody.Statements.Select(s => new StatementWrapper { SyntaxNode = s })));
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // TODO: check if it refering to the same var
        private static (string? loopVariable, int numberOfIterations) HandleForLoopLogic(ForStatementSyntax forSyntax)
        {
            var body = forSyntax.Statement;
            var condition = (BinaryExpressionSyntax)forSyntax.Condition;

            var initializers = forSyntax.Declaration.Variables[0];//TODO this can be empty
            var loopVariable = initializers.Identifier.Value;//TODO check if this one matches the others
            var incrementors = forSyntax.Incrementors;//TODO this can be empty

            //TODO: we're assuming ints - needs to be more generic
            var initialLoopValue = (int)((LiteralExpressionSyntax)initializers.Initializer.Value).Token.Value;
            var operatorKind = condition.Kind();
            var isDecrementRequired = operatorKind == SyntaxKind.LessThanExpression || operatorKind == SyntaxKind.GreaterThanExpression;
            var finalLoopValue = (int)((LiteralExpressionSyntax)condition.Right).Token.Value + (isDecrementRequired ? -1 : 0);
            var incrementorKind = incrementors[0].Kind();
            var increments = incrementorKind == SyntaxKind.PostIncrementExpression ? 1 : incrementorKind == SyntaxKind.PostDecrementExpression ? -1 : throw new NotImplementedException(); ;
            var numberOfIterations = (finalLoopValue - initialLoopValue) / increments;
            return (loopVariable.ToString(), numberOfIterations);
        }

        // TODO: see bellow
        private static MethodWrapper GetMethodCalled(List<(MethodWrapper Method, bool IsConsolidated)> consolidatedMethods, StatementWrapper statement)
        {
            var methodName = statement.Reference.MethodCalled;
            var (methodCalled, _) = consolidatedMethods.FirstOrDefault(m => m.Method.Name == methodName);// TODO: also compare parameters, namespace, etc.
            return methodCalled;
        }

        // TODO: add try catch and throw meaningful exception
        private static void ReplaceStatementByMethodStatements(List<List<StatementWrapper>> currentPaths, IEnumerable<IEnumerable<StatementWrapper>> pathsToAdd)
        {
            var newPaths = new List<List<StatementWrapper>>();

            currentPaths.ForEach(path =>
            {
                path.AddRange(pathsToAdd.First());
                if (pathsToAdd.Count() > 1)
                {
                    newPaths.AddRange(pathsToAdd.Skip(1).Select(pathToAdd => new List<StatementWrapper>(path).Union(pathToAdd).ToList()));
                }
            });

            currentPaths.AddRange(newPaths);
        }

        private static void AddToAll(List<List<StatementWrapper>> listOfLists, StatementWrapper statement) => listOfLists.ForEach(list => list.Add(statement));
    }
}
