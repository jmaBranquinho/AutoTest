namespace AutoTest.CodeInterpreter.Wrappers
{
    public class NamespaceWrapper
    {
        public string Name { get; set; }

        public List<ClassWrapper> Classes { get; set; }

        public void Consolidate(SolutionWrapper solution)
        {
            foreach (var @class in Classes)
            {
                @class.Consolidate(solution);
            }
        }
    }
}
