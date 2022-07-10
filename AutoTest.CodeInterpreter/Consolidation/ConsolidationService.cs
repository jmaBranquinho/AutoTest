using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.CodeInterpreter.Consolidation
{
    public class ConsolidationService
    {
        private List<(MethodWrapper Method, bool IsConsolidated)> _methods = new();

        public void RegisterMethod(MethodWrapper method) => _methods.Add((method, !method.IsConsolidationRequired()));

        public void Consolidate()
        {
            var consolidatedMethods = _methods.Where(m => m.IsConsolidated).ToList();
            var nonConsolidatedMethod = _methods.Except(consolidatedMethods).ToList();

            while (nonConsolidatedMethod.Count != 0)
            {
                var newlyConsolidatedMethods = new List<MethodWrapper>();
                foreach (var method in nonConsolidatedMethod)
                {
                    if (method.Method.GetReferences().Any(m1 => consolidatedMethods.Any(m2 => m2.Method.Name == m1)))
                    {
                         
                        // TODO
                        newlyConsolidatedMethods.Add(method.Method);
                    }
                }
                consolidatedMethods.AddRange(newlyConsolidatedMethods.Select(m => (m, true)));
                nonConsolidatedMethod.RemoveAll(m => newlyConsolidatedMethods.Contains(m.Method));
            }
        }
    }
}
