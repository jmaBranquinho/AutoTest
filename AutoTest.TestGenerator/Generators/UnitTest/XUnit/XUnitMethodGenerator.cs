using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.Core.Models;
using AutoTest.TestGenerator.Generators.Abstracts;
using AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit
{
    public class XUnitMethodGenerator : UnitTestMethodGenerator
    {
        protected override Func<string, IEnumerable<Parameter>, ExecutionPathInfo, UnitTestBase> GenerateUnitTest(MethodWrapper method)
            => method.Parameters.Any()
                ? ((name, parameters, executionPathInfo) => new XUnitTest(name, parameters, executionPathInfo))
                : ((name, _, executionPathInfo) => new XUnitTest(name, executionPathInfo));
    }
}
