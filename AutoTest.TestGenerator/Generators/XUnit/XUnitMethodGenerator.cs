using AutoTest.TestGenerator.Generators.Abstracts;

namespace AutoTest.TestGenerator.Generators.XUnit
{
    public class XUnitMethodGenerator : UnitTestMethodGenerator
    {
        protected override string _parameterlessMethodAnnotation => "[Fact]";
        protected override string _parameterMethodAnnotation => "[Theory]";
        protected override string _parameterAnnotationTemplate => "[InlineData({0})]";

    }
}
