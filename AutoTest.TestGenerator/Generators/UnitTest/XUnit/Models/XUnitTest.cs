using AutoTest.CodeInterpreter.Models;
using AutoTest.Core.Models;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models
{
    public class XUnitTest : Abstracts.UnitTestBase
    {
        protected override string ParameterlessMethodAnnotation => "[Fact]";
        protected override string ParameterMethodAnnotation => "[Theory]";
        protected override string ParameterAnnotationTemplate => "[InlineData({0})]";

        public XUnitTest(string testName, IEnumerable<Parameter> parameters, CodeRunExecution codeRun) : base(testName, parameters, codeRun) { }

        public XUnitTest(string testName, CodeRunExecution codeRun) : base(testName, Enumerable.Empty<Parameter>(), codeRun) { }
    }
}