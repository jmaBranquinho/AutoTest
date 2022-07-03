using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models
{
    public class XUnitTest : Abstracts.UnitTest
    {
        protected override string _parameterlessMethodAnnotation => "[Fact]";
        protected override string _parameterMethodAnnotation => "[Theory]";
        protected override string _parameterAnnotationTemplate => "[InlineData({0})]";

        public XUnitTest(string name, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parameters, IEnumerable<StatementWrapper> methodStatements) : base(name, parameters, methodStatements)
        { }

        public XUnitTest(string name, IEnumerable<StatementWrapper> methodStatements) : base(name, Enumerable.Empty<IEnumerable<(string Name, Type Type, object Value)>>(), methodStatements)
        { }
    }
}