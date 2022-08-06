using AutoTest.CodeInterpreter.Models;
using AutoTest.Core.Models;

namespace AutoTest.TestGenerator.Generators.UnitTest.NUnit.Models
{
    public class NUnitTest : Abstracts.UnitTestBase
    {
        protected override string ParameterlessMethodAnnotation => "[Test]";
        protected override string ParameterMethodAnnotation => string.Empty;
        protected override string ParameterAnnotationTemplate => "[TestCase({0})]";

        public NUnitTest(string name, IEnumerable<Parameter> parameters, ExecutionPathInfo executionPathInfo) : base(name, parameters, executionPathInfo)
        { }

        public NUnitTest(string name, ExecutionPathInfo executionPathInfo) : base(name, Enumerable.Empty<Parameter>(), executionPathInfo)
        { }
    }
}
