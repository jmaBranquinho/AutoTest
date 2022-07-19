using AutoTest.CodeInterpreter.Consolidation;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class NamespaceWrapper
    {
        public string Name { get; set; }

        public List<ClassWrapper> Classes { get; set; }

        public SolutionWrapper Solution { get; set; }

        public void Consolidate(SolutionWrapper solution, ConsolidationService consolidationService)
        {
            foreach (var @class in Classes)
            {
                @class.Consolidate(solution, consolidationService);
            }

            consolidationService.Consolidate();
        }
    }
}
