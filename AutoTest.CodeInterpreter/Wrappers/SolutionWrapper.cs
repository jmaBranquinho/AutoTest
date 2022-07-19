using AutoTest.CodeInterpreter.Consolidation;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class SolutionWrapper
    {
        public string Name { get; set; }

        public List<NamespaceWrapper> Namespaces { get; set; }

        public SolutionWrapper Consolidate()
        {
            var consolidationService = new ConsolidationService();

            foreach (var @namespace in Namespaces)
            {
                @namespace.Consolidate(this, consolidationService);
            }

            return this;
        }
    }
}
