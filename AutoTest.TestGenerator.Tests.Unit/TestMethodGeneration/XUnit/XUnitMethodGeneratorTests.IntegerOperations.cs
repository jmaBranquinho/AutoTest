using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.TestGenerator.Generators.Enums;
using FluentAssertions;
using System.Text.RegularExpressions;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
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
    Assert.Equal(expected, actual);
}
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var method = GetMethodFromExample(_simpleMethodGreaterThan);

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
    Assert.Equal(expected, actual);
}
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var method = GetMethodFromExample(_simpleMethodGreaterThanEquals);

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
    Assert.Equal(expected, actual);
}
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var method = GetMethodFromExample(_simpleMethodLessThan);

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
    Assert.Equal(expected, actual);
}
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var method = GetMethodFromExample(_simpleMethodLessThanEquals);

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
            var method = GetMethodFromExample(_simpleMethodEquals);

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
            var method = GetMethodFromExample(_simpleMethodNotEquals);

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

    }
}
