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

            var isTestingIfBranch = true;
            foreach (var test in result)
            {
                if (isTestingIfBranch)
                {
                    isTestingIfBranch = !isTestingIfBranch;
                    test.ToString().Should().Contain("test");
                }
                else
                {
                    test.ToString().Should().NotContain("test");
                }
            }
        }

        [Fact]
        public void SimpleMethodNotEqualsUsingString()
        {
            var method = GetMethodSyntaxFromExample(_simpleMethodNotEqualsWithString);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isTestingIfBranch = true;
            foreach (var test in result)
            {
                if (isTestingIfBranch)
                {
                    isTestingIfBranch = !isTestingIfBranch;
                    test.ToString().Should().NotContain("test");
                }
                else
                {
                    test.ToString().Should().Contain("test");
                }
            }
        }

        private static readonly string _simpleMethodEqualsWithString = @"
        public int TestMethod(string x) 
        {
            if (x == ""test"")
            {
                return 1;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static readonly string _simpleMethodNotEqualsWithString = @"
        public int TestMethod(string x) 
        {
            if (x != ""test"")
            {
                return 1;
            } 
            else
            {
                return 0;
            }
        }
".Trim();
    }
}
