using AutoTest.CodeInterpreter.Models;
using AutoTest.Core.Models;

namespace AutoTest.TestGenerator.Generators.UnitTest.MsTesting.Models
{
    public class MsUnitTest : Abstracts.UnitTestBase
    {
        protected override string ParameterlessMethodAnnotation => "[TestMethod]";
        protected override string ParameterMethodAnnotation => "[TestMethod]";
        protected override string ParameterAnnotationTemplate => "[DataRow({0})]";

        public MsUnitTest(string name, IEnumerable<Parameter> parameters, ExecutionPathInfo executionPathInfo) : base(name, parameters, executionPathInfo)
        { }

        public MsUnitTest(string name, ExecutionPathInfo executionPathInfo) : base(name, Enumerable.Empty<Parameter>(), executionPathInfo)
        { }
    }
}
