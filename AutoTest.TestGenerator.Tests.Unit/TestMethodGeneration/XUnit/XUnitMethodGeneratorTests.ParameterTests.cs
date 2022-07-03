using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.TestGenerator.Generators.Enums;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void SimpleMethodWithoutParameters()
        {
            var expected = @"
[Fact]
public void TestMethod_WhenSomething_ShouldSomething()
{
    // Arrange
    
    // Act
    var actual = _sut.TestMethod();
    
    // Assert
    Assert.Equal(Assert.Equal(expected, actual);
}
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var method = GetMethodSyntaxFromExample(_simpleMethodWithoutParametersNoLogic);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var test = result.First().ToString();

            UnitTestHelper.AssertSimilarStrings(expected, test);
        }

        [Fact]
        public void SimpleMethodWithParameters()
        {
            var expected = @"
[Theory]
[InlineData(xx)]
public void TestMethod_WhenSomething_ShouldSomething(int x)
{
    // Arrange
    
    // Act
    var actual = _sut.TestMethod(x);
    
    // Assert
    Assert.Equal(Assert.Equal(expected, actual);
}
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var method = GetMethodSyntaxFromExample(_simpleMethodWith1ParameterNoLogic);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var test = result.First().ToString();

            var valueUsedInTest = Regex.Match(test.ToString(), @"[-]*\d+").Value;
            expected = expected.Replace("xx", string.Join(string.Empty, valueUsedInTest));

            UnitTestHelper.AssertSimilarStrings(expected, test);
        }

        private static string _simpleMethodWith1ParameterNoLogic = @"
        public int TestMethod(int x) 
        {
            return x;
        }
".Trim();

        private static string _simpleMethodWithoutParametersNoLogic = @"
        public int TestMethod() 
        {
            return 5;
        }
".Trim();
    }
}
