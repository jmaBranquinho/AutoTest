using AutoTest.TestGenerator.Generators.Enums;
using FluentAssertions;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void SimpleMethodEqualsUsingString()
        {
            var method = GetMethodSyntaxFromExample(_simpleMethodEqualsWithString);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isEqualToTest = true;
            foreach (var test in result)
            {
                if (isEqualToTest)
                {
                    isEqualToTest = !isEqualToTest;
                    test.ToString().Should().Contain("test");
                }
                else
                {
                    test.ToString().Should().NotContain("test");
                }
            }
        }

        private static string _simpleMethodEqualsWithString = @"
        public decimal TestMethod(string x) 
        {
            if (x == ""test"")
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();
    }
}
