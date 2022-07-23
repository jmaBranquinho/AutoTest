using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.CodeInterpreter.Models
{
    public class CodeRunExecution
    {
        public MethodWrapper Method { get; set; }

        public IEnumerable<StatementWrapper> Path { get; set; }

        public IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> Parameters { get; set; }
    }
}
