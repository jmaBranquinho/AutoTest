using AutoTest.TestGenerator.Generators.Enums;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void IfStatements_WhenDealingWithReturns_ElseIsOptional()
        {
            var methodWithElse = GetMethodsFromExample(_methodCallingMethodEquals5Depth5WithComplexity).Skip(4).First();

            var resultWithElse = _sut.GenerateUnitTests(methodWithElse, TestNamingConventions.MethodName_WhenCondition_ShouldResult)
                .Select(test => test.ToString());

            var codeWithoutElse = _methodCallingMethodEquals5Depth5WithComplexity.Replace(@"
            else
            {
                return 0;
            }".Trim(), "return 0;");
            var methodWithoutElse = GetMethodsFromExample(codeWithoutElse).Skip(4).First();

            var resultWithoutElse = _sut.GenerateUnitTests(methodWithoutElse, TestNamingConventions.MethodName_WhenCondition_ShouldResult)
                .Select(test => test.ToString());

            resultWithoutElse.Count().Should().Be(resultWithElse.Count());
            resultWithoutElse.Take(5).Should().BeEquivalentTo(resultWithElse.Take(5));
        }
    }
}
