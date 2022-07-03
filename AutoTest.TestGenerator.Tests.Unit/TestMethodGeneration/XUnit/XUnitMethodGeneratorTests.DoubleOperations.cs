using AutoTest.TestGenerator.Generators.Enums;
using FluentAssertions;
using System.Text.RegularExpressions;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void SimpleMethodEqualsUsingDouble()
        {
            var method = GetMethodSyntaxFromExample(_simpleMethodEqualsWithDouble);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isEqualTo5 = true;
            foreach (var test in result)
            {
                var valueUsedInTest = double.Parse(Regex.Match(test.ToString(), @"[-]*\d+").Value);

                if (isEqualTo5)
                {
                    isEqualTo5 = !isEqualTo5;
                    valueUsedInTest.Should().Be(5);
                }
                else
                {
                    valueUsedInTest.Should().NotBe(5);
                }
            }
        }

        private static string _simpleMethodEqualsWithDouble = @"
        public double TestMethod(double x) 
        {
            if (x == 5d)
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
