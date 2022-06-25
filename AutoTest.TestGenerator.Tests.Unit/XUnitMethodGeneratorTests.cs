using AutoFixture;
using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.CodeInterpreter;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.XUnit;
using System.Linq;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit
{
    public class XUnitMethodGeneratorTests
    {
        private readonly XUnitMethodGenerator _sut;
        private readonly Fixture _fixture;

        public XUnitMethodGeneratorTests()
        {
            _sut = new XUnitMethodGenerator();
            _fixture = new Fixture();
        }

        [Fact]
        public void simple()
        {
            var expected = @"
[Theory]
[InlineData(xx)]
public void TestMethod(int x)
{
    // Arrange
    
    // Act
    
    // Assert
    
}
".Trim();
            var method = GetMethodSyntaxFromExample(_simpleClassAndMethodWithoutParameters);

            var result = new XUnitMethodGenerator().GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var test = result.First().ToString();

            var valueUsedInTest = test.Substring(22).Take(2);
            expected = expected.Replace("xx", string.Join(string.Empty, valueUsedInTest));

            UnitTestHelper.AssertSimilarStrings(expected, test);
        }

        private static MethodWrapper GetMethodSyntaxFromExample(string exampleCode)
        {
            var analyzer = new CodeAnalyzer();
            var solution = analyzer.AnalyzeCode(exampleCode);
            return solution.Namespaces.First().Classes.First().Methods.First();
        }

        private string _simpleClassAndMethodWithoutParameters = @"
namespace TestNameSpace
{
    public class TestClass
    {
        public int TestMethod(int x) 
        {
            return x;
        }
    }
}
".Trim();
    }
}