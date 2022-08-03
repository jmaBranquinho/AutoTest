using AutoTest.CodeInterpreter.Services;

namespace AutoTest.CodeInterpreter.Models.Wrappers
{
    public class SolutionWrapper
    {
        public IEnumerable<NamespaceWrapper> Namespaces { get; set; } = new List<NamespaceWrapper>();

        public SolutionWrapper Consolidate()
        {
            var consolidationService = new ConsolidationService();
            Namespaces?.ToList()?.ForEach(@namespace => @namespace.Consolidate(consolidationService));
            consolidationService.Consolidate();
            return this;
        }
    }
}
