using AutoTest.CodeGenerator.Models;
using AutoTest.CodeInterpreter.Models;

namespace AutoTest.TestGenerator.Generators.UnitTest.MsTesting.Models
{
    public class MsUnitTest : Abstracts.UnitTestBase
    {
        protected override string ParameterlessMethodAnnotation => "[TestMethod]";
        protected override string ParameterMethodAnnotation => "[TestMethod]";
        protected override string ParameterAnnotationTemplate => "[DataRow({0})]";

        public MsUnitTest(string name, IEnumerable<Parameter> parameters, CodeRunExecution codeRun) : base(name, parameters, codeRun)
        { }

        public MsUnitTest(string name, CodeRunExecution codeRun) : base(name, Enumerable.Empty<Parameter>(), codeRun)
        { }
    }
}
