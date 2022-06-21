using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.TestGenerator.Generators.XUnit;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit
{
    public class XUnitMethodGeneratorTests
    {
        [Fact]
        public void OnlyName()
        {
            var expected = @"
[Fact]
public void UnitTestMethod()
{
    // arrange
    
    // act
    
    // assert
    
}
".Trim();

            var method = new XUnitMethodGenerator().GenerateMethod("UnitTestMethod");

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }
    }
}