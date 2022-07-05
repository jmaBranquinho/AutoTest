using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.TestGenerator.Generators.UnitTest.MsTesting.Models
{
    public class MsUnitTest : Abstracts.UnitTest
    {
        protected override string ParameterlessMethodAnnotation => "[TestMethod]";
        protected override string ParameterMethodAnnotation => "[TestMethod]";
        protected override string ParameterAnnotationTemplate => "[DataRow({0})]";

        public MsUnitTest(string name, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parameters, IEnumerable<StatementWrapper> methodStatements) : base(name, parameters, methodStatements)
        { }

        public MsUnitTest(string name, IEnumerable<StatementWrapper> methodStatements) : base(name, Enumerable.Empty<IEnumerable<(string Name, Type Type, object Value)>>(), methodStatements)
        { }
    }
}
