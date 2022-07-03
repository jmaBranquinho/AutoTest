using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.TestGenerator.Generators.UnitTest.NUnit.Models
{
    public class NUnitTest : Abstracts.UnitTest
    {
        protected override string _parameterlessMethodAnnotation => "[Test]";
        protected override string _parameterMethodAnnotation => string.Empty;
        protected override string _parameterAnnotationTemplate => "[TestCase({0})]";

        public NUnitTest(string name, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parameters, IEnumerable<StatementWrapper> methodStatements) : base(name, parameters, methodStatements)
        { }

        public NUnitTest(string name, IEnumerable<StatementWrapper> methodStatements) : base(name, Enumerable.Empty<IEnumerable<(string Name, Type Type, object Value)>>(), methodStatements)
        { }
    }
}
