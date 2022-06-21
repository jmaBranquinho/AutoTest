using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Abstracts;
using System.Text;

namespace AutoTest.TestGenerator.Generators.XUnit
{
    // TODO: refactor this whole class
    public class XUnitMethodGenerator : UnitTestMethodGenerator
    {
        protected override string _parameterlessMethodAnnotation => "[Fact]";
        protected override string _parameterMethodAnnotation => "[Theory]";
        protected override string _parameterAnnotationTemplate => "[InlineData({0})]";

        public override string GenerateMethod(string methodName, MethodWrapper method)
        {
            var methodBody = string.Join(Environment.NewLine, GenerateArrangeSection(), GenerateActSection(), GenerateAssertSection());
            return GenerateMethod(methodName, methodBody);
        }

        private string GenerateArrangeSection()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// arrange");
            // TODO: implement

            return stringBuilder.ToString();
        }

        private string GenerateActSection()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// act");
            // TODO: implement

            return stringBuilder.ToString();
        }

        private string GenerateAssertSection()
        {
                        var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("// assert");
            // TODO: implement

            return stringBuilder.ToString();
        }
    }
}
