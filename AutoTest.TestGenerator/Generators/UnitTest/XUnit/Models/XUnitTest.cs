using AutoTest.CodeInterpreter.Models;
using AutoTest.Core.Models;
using AutoTest.TestGenerator.Generators.Abstracts;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models
{
    public class XUnitTest : UnitTestBase
    {
        protected override string ParameterlessMethodAnnotation => "[Fact]";
        protected override string ParameterMethodAnnotation => "[Theory]";
        protected override string ParameterAnnotationTemplate => "[InlineData({0})]";

        public XUnitTest(string testName, IEnumerable<Parameter> parameters, ExecutionPathInfo executionPathInfo) : base(testName, parameters, executionPathInfo) { }

        public XUnitTest(string testName, ExecutionPathInfo executionPathInfo) : base(testName, Enumerable.Empty<Parameter>(), executionPathInfo) { }
    }
}