using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Abstracts;
using AutoTest.TestGenerator.Generators.XUnit.Models;

namespace AutoTest.TestGenerator.Generators.XUnit
{
    public class XUnitMethodGenerator : UnitTestMethodGenerator
    {
        protected override Func<string, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>>, IEnumerable<StatementWrapper>, UnitTest> GenerateUnitTest(MethodWrapper method) 
            => method.Parameters.Any()
                ? ((name, parameters, statements) => new XUnitTest(name, parameters, statements))
                : ((name, _, statements) => new XUnitTest(name, statements));
    }
}
