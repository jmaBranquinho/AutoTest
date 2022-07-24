using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models
{
    public class XUnitTest : Abstracts.UnitTest
    {
        protected override string ParameterlessMethodAnnotation => "[Fact]";
        protected override string ParameterMethodAnnotation => "[Theory]";
        protected override string ParameterAnnotationTemplate => "[InlineData({0})]";

        public XUnitTest(string name, IEnumerable<(string Name, Type Type, object Value)> parameters, IEnumerable<StatementWrapper> methodStatements) : base(name, parameters, methodStatements)
        { }

        public XUnitTest(string name, IEnumerable<StatementWrapper> methodStatements) : base(name, Enumerable.Empty<(string Name, Type Type, object Value)>(), methodStatements)
        { }
    }
}