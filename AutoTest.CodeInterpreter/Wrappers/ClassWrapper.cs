using AutoTest.CodeInterpreter.Services;

namespace AutoTest.CodeInterpreter.Wrappers
{
    public class ClassWrapper
    {
        public string Name { get; set; } = null!;

        public IEnumerable<MethodWrapper> Methods { get; set; } = new List<MethodWrapper>();

        public void Consolidate(ConsolidationService consolidationService) 
            => Methods
                ?.ToList()
                ?.ForEach(method => 
                    method
                    .AnalyzeMethodDetails()
                    .Consolidate(consolidationService));
    }
}
