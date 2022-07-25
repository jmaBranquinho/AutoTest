using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.TestGenerator.Generators.UnitTest.MsTesting.Models
{
    public class MsUnitTest : Abstracts.UnitTest
    {
        protected override string ParameterlessMethodAnnotation => "[TestMethod]";
        protected override string ParameterMethodAnnotation => "[TestMethod]";
        protected override string ParameterAnnotationTemplate => "[DataRow({0})]";

        public MsUnitTest(string name, IEnumerable<(string Name, Type Type, object Value)> parameters, CodeRunExecution codeRun) : base(name, parameters, codeRun)
        { }

        public MsUnitTest(string name, CodeRunExecution codeRun) : base(name, Enumerable.Empty<(string Name, Type Type, object Value)>(), codeRun)
        { }
    }
}
