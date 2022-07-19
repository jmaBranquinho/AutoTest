using AutoTest.TestGenerator.Generators.Enums;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void MethodCall()
        {
            var method = GetMethodsFromExample(_methodCallingMethodEquals5).Skip(1).First();

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult)
                .Select(test => test.ToString());

            result.Count(test => test.Contains("[InlineData(5)]")).Should().Be(1);
            result.Count(test => !test.Contains("[InlineData(5)]")).Should().Be(1);
        }

        private static readonly string _methodCallingMethodEquals5 = @"
        private int TestMethod1(int x) 
        {
            if (x == 5)
            {
                return 1;
            } 
            else
            {
                return 0;
            }
        }

        public int TestMethod2(int x) 
        {
            return TestMethod1(x);
        }
".Trim();
    }
}
