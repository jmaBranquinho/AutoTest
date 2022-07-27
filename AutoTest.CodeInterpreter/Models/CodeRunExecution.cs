using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Interfaces;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.CodeInterpreter.Models
{
    public class CodeRunExecution
    {
        public MethodWrapper Method { get; set; } = null!;

        public IEnumerable<StatementWrapper> Path { get; set; } = null!;

        public Dictionary<string, IConstraint> ParameterConstraints { get; set; } = null!;
        public Dictionary<string, IValueTracker> VariableConstraints { get; set; } = null!;
        public Parameter ReturnInfo { get; set; } = null!;
    }
}
