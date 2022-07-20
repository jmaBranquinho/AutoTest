using AutoTest.CodeInterpreter.Wrappers;

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
                    if (hasReferencesConsolidated)
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
                    if (statement.HasReference)
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
