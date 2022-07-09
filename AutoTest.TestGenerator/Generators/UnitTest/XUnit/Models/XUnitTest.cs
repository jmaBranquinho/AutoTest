namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models
{
    public class XUnitTest : Abstracts.UnitTest
    {
        protected override string ParameterlessMethodAnnotation => "[Fact]";
        protected override string ParameterMethodAnnotation => "[Theory]";
        protected override string ParameterAnnotationTemplate => "[InlineData({0})]";
    }
}