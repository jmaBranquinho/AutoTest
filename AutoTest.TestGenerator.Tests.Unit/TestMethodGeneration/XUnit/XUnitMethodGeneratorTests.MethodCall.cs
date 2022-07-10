using AutoTest.TestGenerator.Generators.Enums;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void MethodCall()
        {
            var method = GetMethodSyntaxFromExample(_methodCallingMethodEquals5);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            ;
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
