namespace AutoTest.CodeInterpreter.Wrappers
{
    public class SolutionWrapper
    {
        public string Name { get; set; }

        public List<NamespaceWrapper> Namespaces { get; set; }

        public SolutionWrapper Consolidate()
        {
            foreach (var @namespace in Namespaces)
            {
                @namespace.Consolidate(this);
            }

            return this;
        }
    }
}
