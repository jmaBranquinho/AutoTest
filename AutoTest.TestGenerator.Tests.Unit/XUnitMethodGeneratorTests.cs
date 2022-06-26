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

        public XUnitMethodGeneratorTests() => _sut = new XUnitMethodGenerator();

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
".Trim();
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
".Trim();
            var method = GetMethodSyntaxFromExample(_simpleMethodWith1ParameterNoLogic);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var test = result.First().ToString();

            var valueUsedInTest = test.Substring(22).Take(2);
            expected = expected.Replace("xx", string.Join(string.Empty, valueUsedInTest));

            UnitTestHelper.AssertSimilarStrings(expected, test);
        }

        private static MethodWrapper GetMethodSyntaxFromExample(string exampleCode)
        {
            var analyzer = new CodeAnalyzer();
            var solution = analyzer.AnalyzeCode(_classAndNamespaceWrapperTemplate.Replace("{0}", exampleCode));
            return solution.Namespaces.First().Classes.First().Methods.First();
        }

        private static string _classAndNamespaceWrapperTemplate = @"
namespace TestNameSpace
{
    public class TestClass
    {
        {0}
    }
}
".Trim();

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