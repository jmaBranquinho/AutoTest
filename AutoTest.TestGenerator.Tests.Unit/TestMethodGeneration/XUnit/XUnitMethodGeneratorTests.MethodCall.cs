using AutoTest.TestGenerator.Generators.Enums;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void MethodCallDepth1()
        {
            var method = GetMethodsFromExample(_methodCallingMethodEquals5).Skip(1).First();

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult)
                .Select(test => test.ToString());

            result.Count(test => test.Contains("[InlineData(5)]")).Should().Be(1);
            result.Count(test => !test.Contains("[InlineData(5)]")).Should().Be(1);
            result.Count().Should().Be(2);
        }

        [Fact]
        public void MethodCallDepth5()
        {
            var method = GetMethodsFromExample(_methodCallingMethodEquals5Depth5).Skip(1).First();

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult)
                .Select(test => test.ToString());

            result.Count(test => test.Contains("[InlineData(5)]")).Should().Be(1);
            result.Count(test => !test.Contains("[InlineData(5)]")).Should().Be(1);
            result.Count().Should().Be(2);
        }

        [Fact]
        public void MethodCallDepth5WithComplexity()
        {
            var method = GetMethodsFromExample(_methodCallingMethodEquals5Depth5WithComplexity).Skip(1).First();

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult)
                .Select(test => test.ToString());

            result.Count(test => test.Contains("[InlineData(1)]")).Should().Be(1);
            result.Count(test => test.Contains("[InlineData(2)]")).Should().Be(1);
            result.Count(test => test.Contains("[InlineData(3)]")).Should().Be(1);
            result.Count(test => test.Contains("[InlineData(4)]")).Should().Be(1);
            result.Count(test => test.Contains("[InlineData(5)]")).Should().Be(1);
            result.Count().Should().Be(6);
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

        private static readonly string _methodCallingMethodEquals5Depth5 = @"
        private int TestMethod5(int x) 
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

        private int TestMethod4(int x) 
        {
            return TestMethod5(x);
        }

        private int TestMethod3(int x) 
        {
            return TestMethod4(x);
        }

        private int TestMethod2(int x) 
        {
            return TestMethod3(x);
        }

        public int TestMethod1(int x) 
        {
            return TestMethod2(x);
        }
".Trim();

        private static readonly string _methodCallingMethodEquals5Depth5WithComplexity = @"
        private int TestMethod5(int x) 
        {
            if (x == 5)
            {
                return 5;
            } 
            else
            {
                return 0;
            }
        }

        private int TestMethod4(int x) 
        {
            if (x == 4)
            {
                return 4;
            } 
            else
            {
                return TestMethod5(x);
            }
        }

        private int TestMethod3(int x) 
        {
            if (x == 3)
            {
                return 3;
            } 
            else
            {
                return TestMethod4(x);
            }
        }

        private int TestMethod2(int x) 
        {
            if (x == 2)
            {
                return 2;
            } 
            else
            {
                return TestMethod3(x);
            }
        }

        public int TestMethod1(int x) 
        {
            if (x == 1)
            {
                return 1;
            } 
            else
            {
                return TestMethod2(x);
            }
        }
".Trim();
    }
}
