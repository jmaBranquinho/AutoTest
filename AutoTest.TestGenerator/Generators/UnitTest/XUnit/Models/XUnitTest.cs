using AutoTest.CodeInterpreter.Models;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models
{
    public class XUnitTest : Abstracts.UnitTest
    {
        protected override string ParameterlessMethodAnnotation => "[Fact]";
        protected override string ParameterMethodAnnotation => "[Theory]";
        protected override string ParameterAnnotationTemplate => "[InlineData({0})]";

        public XUnitTest(string testName, IEnumerable<(string Name, Type Type, object Value)> parameters, CodeRunExecution codeRun) : base(testName, parameters, codeRun)
        { }

        public XUnitTest(string testName, CodeRunExecution codeRun) : base(testName, Enumerable.Empty<(string Name, Type Type, object Value)>(), codeRun)
        { }
    }
}