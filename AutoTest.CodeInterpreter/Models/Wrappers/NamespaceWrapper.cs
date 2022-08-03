using AutoTest.CodeInterpreter.Services;

namespace AutoTest.CodeInterpreter.Models.Wrappers
{
    public class NamespaceWrapper
    {
        public string Name { get; set; } = null!;

        public IEnumerable<ClassWrapper> Classes { get; set; } = new List<ClassWrapper>();

        public void Consolidate(ConsolidationService consolidationService)
            => Classes?.ToList()?.ForEach(@class => @class.Consolidate(consolidationService));
    }
}
