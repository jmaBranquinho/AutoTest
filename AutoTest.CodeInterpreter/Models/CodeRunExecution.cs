using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.CodeInterpreter.Models
{
    public class CodeRunExecution
    {
        public MethodWrapper Method { get; set; }

        public IEnumerable<StatementWrapper> Path { get; set; }

        public Dictionary<string, IConstraint> ParameterConstraints { get; set; }
    }
}
