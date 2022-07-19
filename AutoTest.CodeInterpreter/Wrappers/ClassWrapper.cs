using AutoTest.CodeInterpreter.Consolidation;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class ClassWrapper
    {
        public string Name { get; set; }

        public List<MethodWrapper> Methods { get; set; }

        public NamespaceWrapper Namespace { get; set; }

        public void Consolidate(SolutionWrapper solution, ConsolidationService consolidationService)
        {
            foreach (var method in Methods)
            {
                method.Consolidate(solution, consolidationService);
            }
        }
    }
}
