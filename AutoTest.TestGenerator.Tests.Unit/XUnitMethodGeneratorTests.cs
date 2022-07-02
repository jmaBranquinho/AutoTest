using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.CodeInterpreter;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.XUnit;
using FluentAssertions;
using System;
using System.Linq;
using System.Text.RegularExpressions;
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

            var valueUsedInTest = test.Substring(22).Take(2);
            expected = expected.Replace("xx", string.Join(string.Empty, valueUsedInTest));

            UnitTestHelper.AssertSimilarStrings(expected, test);
        }

        [Fact]
        public void SimpleMethodGreaterThan()
        {
            var expectedTemplate = @"
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
            var method = GetMethodSyntaxFromExample(_simpleMethodGreaterThan);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isBiggerThen5 = true;
            foreach (var test in result)
            {
                var valueUsedInTest = Regex.Match(test.ToString(), @"[-]*\d+").Value;

                if(isBiggerThen5)
                {
                    isBiggerThen5 = !isBiggerThen5;
                    int.Parse(valueUsedInTest).Should().BeGreaterThan(5);
                    int.Parse(valueUsedInTest).Should().BeLessThan(100);
                } 
                else
                {
                    int.Parse(valueUsedInTest).Should().BeLessThanOrEqualTo(5);
                    int.Parse(valueUsedInTest).Should().BeGreaterThanOrEqualTo(0);
                }

                var expected = expectedTemplate.Replace("xx", string.Join(string.Empty, valueUsedInTest));

                UnitTestHelper.AssertSimilarStrings(expected, test.ToString());
            }
        }

        [Fact]
        public void SimpleMethodGreaterThanEquals()
        {
            var expectedTemplate = @"
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
            var method = GetMethodSyntaxFromExample(_simpleMethodGreaterThanEquals);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isBiggerThen5 = true;
            foreach (var test in result)
            {
                var valueUsedInTest = Regex.Match(test.ToString(), @"[-]*\d+").Value;

                if (isBiggerThen5)
                {
                    isBiggerThen5 = !isBiggerThen5;
                    int.Parse(valueUsedInTest).Should().BeGreaterThanOrEqualTo(5);
                    int.Parse(valueUsedInTest).Should().BeLessThan(100);
                }
                else
                {
                    int.Parse(valueUsedInTest).Should().BeLessThan(5);
                    int.Parse(valueUsedInTest).Should().BeGreaterThanOrEqualTo(0);
                }

                var expected = expectedTemplate.Replace("xx", string.Join(string.Empty, valueUsedInTest));

                UnitTestHelper.AssertSimilarStrings(expected, test.ToString());
            }
        }

        [Fact]
        public void SimpleMethodLessThan()
        {
            var expectedTemplate = @"
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
            var method = GetMethodSyntaxFromExample(_simpleMethodLessThan);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isBiggerThen5 = true;
            foreach (var test in result)
            {
                var valueUsedInTest = Regex.Match(test.ToString(), @"[-]*\d+").Value;

                if (isBiggerThen5)
                {
                    isBiggerThen5 = !isBiggerThen5;
                    int.Parse(valueUsedInTest).Should().BeGreaterThanOrEqualTo(5);
                    int.Parse(valueUsedInTest).Should().BeLessThan(100);
                }
                else
                {
                    int.Parse(valueUsedInTest).Should().BeLessThan(5);
                    int.Parse(valueUsedInTest).Should().BeGreaterThanOrEqualTo(0);
                }

                var expected = expectedTemplate.Replace("xx", string.Join(string.Empty, valueUsedInTest));

                UnitTestHelper.AssertSimilarStrings(expected, test.ToString());
            }
        }

        [Fact]
        public void SimpleMethodLessThanOrEqual()
        {
            var expectedTemplate = @"
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
            var method = GetMethodSyntaxFromExample(_simpleMethodLessThanEquals);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isBiggerThen5 = true;
            foreach (var test in result)
            {
                var valueUsedInTest = Regex.Match(test.ToString(), @"[-]*\d+").Value;

                if (isBiggerThen5)
                {
                    isBiggerThen5 = !isBiggerThen5;
                    int.Parse(valueUsedInTest).Should().BeGreaterThan(5);
                    int.Parse(valueUsedInTest).Should().BeLessThan(100);
                }
                else
                {
                    int.Parse(valueUsedInTest).Should().BeLessThanOrEqualTo(5);
                    int.Parse(valueUsedInTest).Should().BeGreaterThanOrEqualTo(0);
                }

                var expected = expectedTemplate.Replace("xx", string.Join(string.Empty, valueUsedInTest));

                UnitTestHelper.AssertSimilarStrings(expected, test.ToString());
            }
        }

        [Fact]
        public void SimpleMethodEquals()
        {
            var method = GetMethodSyntaxFromExample(_simpleMethodEquals);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isEqualTo5 = true;
            foreach (var test in result)
            {
                var valueUsedInTest = int.Parse(Regex.Match(test.ToString(), @"[-]*\d+").Value);

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

        [Fact]
        public void SimpleMethodNotEquals()
        {
            var method = GetMethodSyntaxFromExample(_simpleMethodNotEquals);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var isNotEqualTo5 = true;
            foreach (var test in result)
            {
                var valueUsedInTest = int.Parse(Regex.Match(test.ToString(), @"[-]*\d+").Value);

                if (isNotEqualTo5)
                {
                    isNotEqualTo5 = !isNotEqualTo5;
                    valueUsedInTest.Should().NotBe(5);
                }
                else
                {
                    valueUsedInTest.Should().Be(5);
                }
            }
        }

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

        [Fact]
        public void SimpleMethodEqualsUsingDecimal()
        {
            var method = GetMethodSyntaxFromExample(_simpleMethodEqualsWithDecimal);

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

        private static string _simpleMethodGreaterThan = @"
        public int TestMethod(int x) 
        {
            if (x > 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodGreaterThanEquals = @"
        public int TestMethod(int x) 
        {
            if (x >= 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodLessThan = @"
        public int TestMethod(int x) 
        {
            if (x < 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodLessThanEquals = @"
        public int TestMethod(int x) 
        {
            if (x <= 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodEquals = @"
        public int TestMethod(int x) 
        {
            if (x == 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodNotEquals = @"
        public int TestMethod(int x) 
        {
            if (x != 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

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

        private static string _simpleMethodEqualsWithDecimal = @"
        public decimal TestMethod(decimal x) 
        {
            if (x == 5m)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodWith1ParameterIfStatementType1 = @"
        public int TestMethod(int x) 
        {
            if (x > 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodWith1ParameterIfStatementType2 = @"
        public int TestMethod(int x) 
        {
            if (x > 5) 
            {
                return x;
            }
            return 0;
        }
".Trim();

        private static string _simpleMethodWith1ParameterIfStatementType3 = @"
        public int TestMethod(int x) 
        {
            return x > 5 ? x : 0;
        }
".Trim();
    }
}