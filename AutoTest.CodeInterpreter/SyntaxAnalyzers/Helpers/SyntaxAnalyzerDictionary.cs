using AutoTest.CodeInterpreter.Interfaces;

namespace AutoTest.CodeInterpreter.SyntaxAnalyzers.Helpers
{
    public class SyntaxAnalyzerDictionary
    {
        private Dictionary<Type, ISyntaxAnalyzer> _dictionary;
        private ISyntaxAnalyzer _fallback;

        public SyntaxAnalyzerDictionary() => BuildDictionary();

        public ISyntaxAnalyzer GetAnalyzerFromDictionary(Type type) => _dictionary.TryGetValue(type, out var analyzer) ? analyzer : _fallback;

        public void BuildDictionary()
        {
            _dictionary = new Dictionary<Type, ISyntaxAnalyzer>();

            var analyzers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ISyntaxAnalyzer).IsAssignableFrom(p))
                .ToList();

            foreach (var analyzer in analyzers)
            {
                // TODO check nulls

                if (analyzer.IsInterface || analyzer.IsAbstract) continue;

                var instance = (ISyntaxAnalyzer)Activator.CreateInstance(analyzer);
                if (instance.ReferredType is not null)
                {
                    _dictionary.Add(instance.ReferredType, instance);
                }
                else
                {
                    _fallback = instance;
                }
            }
        }
    }
}
