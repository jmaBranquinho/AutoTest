using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.Core.Models;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.CodeInterpreter.Models
{
    public class CodeRunExecution
    {
        public MethodWrapper Method { get; set; } = null!;

        public IEnumerable<StatementWrapper> Path { get; set; } = null!;

        public Dictionary<string, IConstraint> ParameterConstraints { get; set; } = null!;

        public LiteralOrParameterDefinition? ReturnParameter { get; set; }
    }
}
