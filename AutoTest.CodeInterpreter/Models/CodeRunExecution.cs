using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.CodeInterpreter.Models
{
    public class CodeRunExecution
    {
        public MethodWrapper Method { get; set; }

        public IEnumerable<StatementWrapper> Path { get; set; }

        public Dictionary<string, IConstraint> ParameterConstraints { get; set; }
        public Dictionary<string, IValueTracker> VariableConstraints { get; set; }
        public (string name, Type type, object? value) ReturnInfo { get; set; }
    }
}
